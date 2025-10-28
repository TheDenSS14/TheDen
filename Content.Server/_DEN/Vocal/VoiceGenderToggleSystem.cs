using Content.Server.Actions;
using Content.Shared._DEN.Vocal;
using Content.Shared.Actions;
using Content.Shared.Humanoid;
using Content.Shared.Speech.Components;

namespace Content.Server._DEN.Vocal;

public sealed partial class VoiceGenderToggleSystem : SharedVoiceGenderToggleSystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly ActionContainerSystem _actionContainer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<VoiceGenderToggleComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<VoiceGenderToggleComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<VoiceGenderToggleComponent, ToggleVoiceGenderEvent>(OnToggleVoiceGender);
        SubscribeLocalEvent<VoiceGenderToggleComponent, VoiceChangedEvent>(OnVoiceChanged);
    }

    private void OnMapInit(Entity<VoiceGenderToggleComponent> ent, ref MapInitEvent args)
    {
        _actions.AddAction(ent, ref ent.Comp.ToggleActionEntity, ent.Comp.ToggleAction);

        var sex = Sex.Unsexed; // i guess
        if (TryComp<HumanoidAppearanceComponent>(ent.Owner, out var humanoid))
            sex = humanoid.Sex;

        ent.Comp.CurrentVoiceGender = sex;

        UpdateToggleAction(ent);
    }

    private void OnComponentShutdown(Entity<VoiceGenderToggleComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.ToggleActionEntity is not null)
            _actionContainer.RemoveAction(ent.Comp.ToggleActionEntity.Value);
    }

    private void UpdateToggleAction(Entity<VoiceGenderToggleComponent> ent)
    {
        var actionEnt = ent.Comp.ToggleActionEntity;
        _actions.SetToggled(actionEnt, ent.Comp.CurrentVoiceGender == Sex.Female);
    }

    private void OnToggleVoiceGender(Entity<VoiceGenderToggleComponent> ent, ref ToggleVoiceGenderEvent args)
    {
        var gender = ent.Comp.CurrentVoiceGender == Sex.Male
            ? Sex.Female
            : Sex.Male;

        var voiceEvent = new VoiceChangedEvent(ent.Comp.CurrentVoiceGender, gender);
        RaiseLocalEvent(ent, voiceEvent);

        args.Handled = true;
    }

    private void OnVoiceChanged(Entity<VoiceGenderToggleComponent> ent, ref VoiceChangedEvent args)
    {
        ent.Comp.CurrentVoiceGender = args.NewVoice;
        UpdateToggleAction(ent);
    }
}
