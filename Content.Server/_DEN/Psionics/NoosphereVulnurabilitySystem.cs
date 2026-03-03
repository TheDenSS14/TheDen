using Content.Server.Abilities.Psionics;
using Content.Server.Chat.Systems;
using Content.Shared._DEN.Traits;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Chat;
using Content.Shared.Cloning;
using Content.Shared.Medical;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Psionics.Glimmer;


namespace Content.Server._DEN.Psionics;

public sealed class NoosphereVulnurabilitySystem : EntitySystem
{
    [Dependency] private readonly PsionicAbilitiesSystem _psionics = default!;
    [Dependency] private readonly GlimmerSystem _glimmer = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NoosphereVulnerabilityComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<NoosphereVulnerabilityComponent, MobStateChangedEvent>(OnDeath);
        SubscribeLocalEvent<NoosphereVulnerabilityComponent, AttemptCloningEvent>(OnAttemptCloning);
        SubscribeLocalEvent<NoosphereVulnerabilityComponent, UpdateMobStateEvent>(OnReviveAttempt, before:[typeof(MobThresholdSystem)]);
        SubscribeLocalEvent<NoosphereVulnerabilityComponent, TargetBeforeDefibrillatorZapsEvent>(OnDefibAttempt);
    }

    private void OnMapInit(Entity<NoosphereVulnerabilityComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<MobStateComponent>(ent, out var mobState))
            return;

        ent.Comp.OriginalAllowedDamageStates = mobState.AllowedStates;
    }

    private void OnDefibAttempt(Entity<NoosphereVulnerabilityComponent> ent, ref TargetBeforeDefibrillatorZapsEvent args)
    {
        if (_glimmer.GlimmerOutput >= ent.Comp.ResurrectionThreshold)
            return;

        args.Cancel();
        _chat.TrySendInGameICMessage(
            args.Defib,
            Loc.GetString(ent.Comp.UnresurrectableMessage),
            InGameICChatType.Speak,
            true);
    }

    private void OnReviveAttempt(Entity<NoosphereVulnerabilityComponent> ent, ref UpdateMobStateEvent args)
    {
        // essentially checks if this mob state change is actually a revive
        if (args.Component.CurrentState != MobState.Dead || args.State == MobState.Dead)
            return;

        // kind of hacky but we work with what we got
        if (_glimmer.GlimmerOutput >= ent.Comp.ResurrectionThreshold)
        {
            args.Component.AllowedStates = ent.Comp.OriginalAllowedDamageStates;
            return;
        }

        args.Component.AllowedStates.RemoveWhere(a => a is > MobState.Invalid and < MobState.Dead);
        args.State = MobState.Dead;
        _chat.TrySendInGameICMessage(
            args.Target,
            Loc.GetString(ent.Comp.FailedReviveAttemptMessage),
            InGameICChatType.Emote,
            true,
            ignoreActionBlocker: true);
    }

    private void OnDeath(Entity<NoosphereVulnerabilityComponent> ent, ref MobStateChangedEvent args)
    {
        if (!TryComp<PsionicComponent>(ent, out var psionic))
            return;

        if(args.NewMobState == MobState.Dead)
        {
            TryComp<InnatePsionicPowersComponent>(ent, out var innatePowers);
            foreach (var power in psionic.ActivePowers)
            {
                if (!innatePowers?.PowersToAdd.Contains(power) ?? true)
                    _psionics.RemovePsionicPower(ent, power);
            }
        }
    }

    private void OnAttemptCloning(Entity<NoosphereVulnerabilityComponent> ent, ref AttemptCloningEvent args)
    {
        if (args.CloningFailMessage is not null
            || args.Cancelled)
            return;

        if (_glimmer.GlimmerOutput >= ent.Comp.ResurrectionThreshold)
            return;

        args.CloningFailMessage = ent.Comp.UnresurrectableMessage;
        args.Cancelled = true;
    }

}
