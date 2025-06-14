using System.Diagnostics.CodeAnalysis;
using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.Silicons.Bots;

namespace Content.Server.Silicons.Bots;

public sealed class PlantbotSystem : SharedPlantbotSystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly PlantHolderSystem _plantHolder = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlantBotWateringDoAfterEvent>(OnDoWaterPlant);
        SubscribeLocalEvent<PlantBotWeedingDoAfterEvent>(OnDoWeedPlant);
        SubscribeLocalEvent<PlantBotDrinkingDoAfterEvent>(OnDoDrinkPlant);
    }

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot watering a plant. Does not actually check
    ///     if the plant needs watering; use <see cref="CanWaterPlantHolder"> to check.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoWaterPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
        => TryDoPlantMaintenance<PlantBotWateringDoAfterEvent>(plantBot, plantHolder, "plantbot-add-water");

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot removing weeds from a plant. Does not actually check
    ///     if the plant needs weeding; use <see cref="CanWeedPlantHolder"> to check.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoWeedPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
        => TryDoPlantMaintenance<PlantBotWeedingDoAfterEvent>(plantBot, plantHolder, "plantbot-remove-weeds");

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot drinking water out of a plant. Does not actually check
    ///     if the plantbot can drink; use <see cref="CanDrinkPlant"> to check. This is emagged plantbot behavior.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoDrinkPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        if (!CanDrinkPlant(plantBot, plantHolder))
            return;

        TryDoPlantMaintenance<PlantBotDrinkingDoAfterEvent>(plantBot, plantHolder);
    }
    private void OnDoWaterPlant(ref PlantBotWateringDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, WaterPlant);

    private void OnDoWeedPlant(ref PlantBotWeedingDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, WeedPlant);

    private void OnDoDrinkPlant(ref PlantBotDrinkingDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, DrinkPlant);

    private void OnDoPlantMaintenance<TEvent>(ref TEvent args,
        Action<Entity<PlantbotComponent>, Entity<PlantHolderComponent>> action)
        where TEvent : DoAfterEvent
    {
        var target = args.Target;
        if (target == null || !TryGetBotAndHolder(args.User, target.Value, out var bot, out var holder))
            return;

        action(bot.Value, holder.Value);
        args.Handled = true;
    }

    private void TryDoPlantMaintenance<TEvent>(Entity<PlantbotComponent> plantBot,
        Entity<PlantHolderComponent> plantHolder,
        LocId? speakText = null)
        where TEvent : DoAfterEvent, new()
    {
        if (speakText != null)
            _chat.TrySendInGameICMessage(plantBot.Owner,
                Loc.GetString(speakText),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);

        var doAfterEventArgs = GetActionArgs(plantBot, plantHolder, new TEvent());
        _doAfter.TryStartDoAfter(doAfterEventArgs);
    }

    public void WaterPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        _plantHolder.AdjustWater(plantHolder.Owner, plantBot.Comp.WaterTransferAmount, plantHolder.Comp);
        AudioSystem.PlayPvs(plantBot.Comp.WaterSound, plantHolder.Owner);
    }

    public void WeedPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        plantHolder.Comp.WeedLevel -= plantBot.Comp.WeedsRemovedAmount;
        AudioSystem.PlayPvs(plantBot.Comp.WeedSound, plantHolder.Owner);
    }

    public void DrinkPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        _plantHolder.AdjustWater(plantHolder.Owner, -plantBot.Comp.WaterTransferAmount, plantHolder.Comp);
        AudioSystem.PlayPvs(plantBot.Comp.RemoveWaterSound, plantHolder.Owner);
    }

    private DoAfterArgs GetActionArgs(Entity<PlantbotComponent> plantBot,
        Entity<PlantHolderComponent> plantHolder,
        DoAfterEvent @event,
        float doAfterLength = 1.2f)
    {
        var doAfterEventArgs = new DoAfterArgs(EntityManager,
            plantBot.Owner,
            doAfterLength,
            @event,
            plantBot.Owner,
            plantHolder.Owner)
        {
            BreakOnMove = plantHolder.Owner != plantBot.Owner,
            BreakOnWeightlessMove = false,
            NeedHand = false,
            Broadcast = true
        };

        return doAfterEventArgs;
    }

    private bool TryGetBotAndHolder(EntityUid botId,
        EntityUid holderId,
        [NotNullWhen(true)] out Entity<PlantbotComponent>? bot,
        [NotNullWhen(true)] out Entity<PlantHolderComponent>? holder)
    {
        bot = null;
        holder = null;

        if (!TryComp<PlantbotComponent>(botId, out var plantBot)
            || !TryComp<PlantHolderComponent>(holderId, out var plantHolder))
            return false;

        bot = new Entity<PlantbotComponent>(botId, plantBot);
        holder = new Entity<PlantHolderComponent>(holderId, plantHolder);
        return true;
    }

    /// <summary>
    ///     Whether or not a plant holder needs maintenance from a plantbot.
    /// </summary>
    /// <param name="plantBot">The plantbot who will perform the maintenance.</param>
    /// <param name="plantHolder">The plant holder (hydroponics tray, soil plot, etc).</param>
    /// <returns>If the plantbot should perform maintenance on the plant holder.</returns>
    public bool CanServicePlantHolder(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        return CanWaterPlantHolder(plantBot, plantHolder)
            || CanWeedPlantHolder(plantBot, plantHolder)
            || CanDrinkPlant(plantBot, plantHolder);
    }

    /// <summary>
    ///     Whether or not a plantbot will want to water this plant holder.
    /// </summary>
    /// <param name="plantBot">The plantbot who will water the plant.</param>
    /// <param name="plantHolder">The plant holder (hydroponics tray, soil plot, etc).</param>
    /// <returns>If the plantbot should water the plant holder.</returns>
    public bool CanWaterPlantHolder(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        return plantHolder.Comp.WaterLevel < plantBot.Comp.RequiredWaterLevelToService;
    }

    /// <summary>
    ///     Whether or not a plantbot will want to remove weeds from this plant holder.
    /// </summary>
    /// <param name="plantBot">The plantbot who will weed the plant.</param>
    /// <param name="plantHolder">The plant holder (hydroponics tray, soil plot, etc).</param>
    /// <returns>If the plantbot should weed the plant holder.</returns>
    public bool CanWeedPlantHolder(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        return plantHolder.Comp.WeedLevel >= plantBot.Comp.RequiredWeedsAmountToWeed;
    }

    /// <summary>
    ///     Whether or not a plantbot will want to drink from this plant holder.
    /// </summary>
    /// <param name="plantBot">The plantbot who will drink from the plant.</param>
    /// <param name="plantHolder">The plant holder (hydroponics tray, soil plot, etc).</param>
    /// <returns>If the plantbot would want to drink from the plant holder.</returns>
    public bool CanDrinkPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        return HasComp<EmaggedComponent>(plantBot.Owner)
            && plantHolder.Comp.WaterLevel >= 0f
            && !plantHolder.Comp.Dead;
    }
}
