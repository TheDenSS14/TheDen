using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Emag.Systems;
using Content.Shared.Mobs.Components;
using Content.Shared.Silicons.Bots;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Silicons.Bots;

public sealed class MedibotSystem : SharedMedibotSystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedHypospraySystem _hypospraySystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EmaggableMedibotComponent, GotEmaggedEvent>(OnEmagged);
        SubscribeLocalEvent<MedibotInjectTargetEvent>(OnInjectTarget);
    }

    private void OnEmagged(EntityUid uid, EmaggableMedibotComponent comp, ref GotEmaggedEvent args)
    {
        if (!TryComp<MedibotComponent>(uid, out var medibot))
            return;

        _audio.PlayPredicted(comp.SparkSound, uid, args.UserUid);

        foreach (var (state, treatment) in comp.Replacements)
        {
            medibot.Treatments[state] = treatment;
        }

        args.Handled = true;
    }

    private void OnInjectTarget(MedibotInjectTargetEvent ev)
    {
        if (ev.Handled
            || !TryComp<MedibotComponent>(ev.Performer, out var medibot)
            || !TryComp<MobStateComponent>(ev.Target, out var state)
            || !TryGetTreatment(medibot, state.CurrentState, out var treatment))
            return;

    }
}
