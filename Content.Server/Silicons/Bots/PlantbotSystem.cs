// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server.Actions;
using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Shared.Actions;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Silicons.Bots;

namespace Content.Server.Silicons.Bots;

public sealed class PlantbotSystem : SharedPlantbotSystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly PlantHolderSystem _plantHolder = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlantbotComponent, ComponentStartup>(OnPlantbotStartup);
        SubscribeLocalEvent<PlantbotWateringDoAfterEvent>(OnDoWaterPlant);
        SubscribeLocalEvent<PlantbotWeedingDoAfterEvent>(OnDoWeedPlant);
        SubscribeLocalEvent<PlantbotDrinkingDoAfterEvent>(OnDoDrinkPlant);
        SubscribeLocalEvent<PlantbotWaterPlantActionEvent>(OnWaterPlantAction);
        SubscribeLocalEvent<PlantbotRemoveWeedsActionEvent>(OnRemoveWeedsAction);
    }

    private void OnPlantbotStartup(EntityUid uid, PlantbotComponent component, ComponentStartup args)
    {
        _actions.AddAction(uid, ref component.WaterPlantActionEntity, component.WaterPlantActionId);
        _actions.AddAction(uid, ref component.WeedPlantActionEntity, component.WeedPlantActionId);
    }

    private void OnWaterPlantAction(ref PlantbotWaterPlantActionEvent args)
        => HandlePlantMaintenanceAction(ref args,
            CanWaterPlantHolder,
            TryDoWaterPlant,
            "plantbot-error-too-much-water");

    private void OnRemoveWeedsAction(ref PlantbotRemoveWeedsActionEvent args)
        => HandlePlantMaintenanceAction(ref args,
            CanWeedPlantHolder,
            TryDoWeedPlant,
            "plant-holder-component-no-weeds-message");

    private void OnDoWaterPlant(ref PlantbotWateringDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, WaterPlant);

    private void OnDoWeedPlant(ref PlantbotWeedingDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, WeedPlant);

    private void OnDoDrinkPlant(ref PlantbotDrinkingDoAfterEvent args)
        => OnDoPlantMaintenance(ref args, DrinkPlant);

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot watering a plant. Does not actually check
    ///     if the plant needs watering; use <see cref="CanWaterPlantHolder"> to check.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoWaterPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
        => TryDoPlantMaintenance<PlantbotWateringDoAfterEvent>(plantBot, plantHolder, "plantbot-add-water");

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot removing weeds from a plant. Does not actually check
    ///     if the plant needs weeding; use <see cref="CanWeedPlantHolder"> to check.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoWeedPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
        => TryDoPlantMaintenance<PlantbotWeedingDoAfterEvent>(plantBot, plantHolder, "plantbot-remove-weeds");

    /// <summary>
    ///     Starts a DoAfter that will end in the plantBot drinking water out of a plant. Does not actually check
    ///     if the plantbot can drink; use <see cref="CanDrinkPlant"> to check. This is emagged plantbot behavior.
    /// </summary>
    /// <param name="plantBot">The plantBot that will perform the maintenance.</param>
    /// <param name="plantHolder">The plantHolder (hydroponics tray etc) that will receive maintenance.</param>
    public void TryDoDrinkPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
        => TryDoPlantMaintenance<PlantbotDrinkingDoAfterEvent>(plantBot, plantHolder);

    private void OnDoPlantMaintenance<TEvent>(ref TEvent args,
        Action<Entity<PlantbotComponent>, Entity<PlantHolderComponent>> action)
        where TEvent : DoAfterEvent
    {
        var target = args.Target;
        if (target == null
            || args.Handled
            || args.Cancelled
            || !TryGetBotAndHolder(args.User, target.Value, out var bot, out var holder))
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

    private void HandlePlantMaintenanceAction<TEvent>(ref TEvent args,
        Func<Entity<PlantbotComponent>, Entity<PlantHolderComponent>, bool> condition,
        Action<Entity<PlantbotComponent>, Entity<PlantHolderComponent>> actionFunction,
        LocId? failedMessage = null)
        where TEvent : EntityTargetActionEvent
    {
        if (args.Handled ||
            !TryGetBotAndHolder(args.Performer, args.Target, out var bot, out var holder))
            return;

        if (CanDrinkPlant(bot.Value, holder.Value))
        {
            TryDoDrinkPlant(bot.Value, holder.Value);
            args.Handled = true;
            return;
        }

        if (condition(bot.Value, holder.Value))
        {
            actionFunction(bot.Value, holder.Value);
            args.Handled = true;
        }
        else if (failedMessage != null)
            _popup.PopupCursor(Loc.GetString(failedMessage), args.Performer);
    }

    public void WaterPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        _popup.PopupCursor(Loc.GetString("plantbot-add-water-message",
            ("name", Identity.Name(plantHolder.Owner, EntityManager))),
            plantBot.Owner,
            PopupType.Medium);
        _plantHolder.AdjustWater(plantHolder.Owner, plantBot.Comp.WaterTransferAmount, plantHolder.Comp);
        AudioSystem.PlayPvs(plantBot.Comp.WaterSound, plantHolder.Owner);
    }

    public void WeedPlant(Entity<PlantbotComponent> plantBot, Entity<PlantHolderComponent> plantHolder)
    {
        _popup.PopupCursor(Loc.GetString("plant-holder-component-remove-weeds-message",
            ("name", Identity.Name(plantHolder.Owner, EntityManager))),
            plantBot.Owner,
            PopupType.Medium);
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
        DoAfterEvent @event)
    {
        var doAfterEventArgs = new DoAfterArgs(EntityManager,
            plantBot.Owner,
            plantBot.Comp.DoAfterLength,
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
        if (plantBot.Comp.IsEmagged)
            return CanDrinkPlant(plantBot, plantHolder);

        return CanWaterPlantHolder(plantBot, plantHolder)
            || CanWeedPlantHolder(plantBot, plantHolder);
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
            && plantHolder.Comp.WaterLevel > 0f
            && !plantHolder.Comp.Dead;
    }
}
