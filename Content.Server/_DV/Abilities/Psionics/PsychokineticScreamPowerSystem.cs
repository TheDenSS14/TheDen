using Content.Shared._DV.Abilities;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Actions;
using Robust.Server.Audio;

namespace Content.Server._DV.Abilities;

public sealed partial class PsychokineticScreamPowerSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly ShatterLightsAbilitySystem _shatterLights = default!;

    public override void Initialize()
    {
        base.Initialize();

        //SubscribeLocalEvent<PsychokineticScreamPowerComponent, MapInitEvent>(OnMapInit);
        //SubscribeLocalEvent<PsychokineticScreamPowerComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<PsychokineticScreamPowerComponent, ShatterLightsActionEvent>(OnShatterLightsAction);
    }


    private void OnShatterLightsAction(Entity<PsychokineticScreamPowerComponent> entity, ref ShatterLightsActionEvent args)
    {
        if (args.Handled)
            return;

        if (entity.Comp.AbilitySound != null)
            _audio.PlayPvs(entity.Comp.AbilitySound, entity);

        _shatterLights.ShatterLightsAround(entity.Owner, entity.Comp.Radius, entity.Comp.LineOfSight);
        args.Handled = true;
    }

}
