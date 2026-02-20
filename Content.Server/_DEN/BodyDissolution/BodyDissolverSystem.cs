using Content.Server.Atmos.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Coordinates;
using Content.Shared.Mobs.Systems;
using Content.Shared.Projectiles;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Content.Shared.Atmos;
using Content.Shared.Body.Components;
using Robust.Shared.Physics.Components;
using Content.Shared.Emag.Systems;
using Content.Shared.Emag.Components;
using Robust.Shared.Timing;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Server.Paint;
using Robust.Shared.Prototypes;

namespace Content.Server.BodyDissolution
{
    public sealed class BodyDissolutionSystem : EntitySystem
    {
        [Dependency] private readonly SharedAudioSystem _sharedAudioSystem = default!;
        [Dependency] private readonly PuddleSystem _puddleSystem = default!;
        [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
        [Dependency] private readonly SharedChatSystem _sharedChatSystem = default!;
        [Dependency] private readonly SharedSolutionContainerSystem _sharedSolutionContainerSystem = default!;
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly SharedPopupSystem _sharedPopupSystem = default!;
        [Dependency] private readonly PaintSystem _paintSystem = default!;
        [Dependency] private readonly IPrototypeManager _proto = default!;
        [Dependency] private readonly IGameTiming _timing = default!;

        private readonly HashSet<EntityUid> _queuedDestroyTacks = new();

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<BodyDissolverComponent, EmbedEvent>(OnEmbed);
            SubscribeLocalEvent<BodyDissolverComponent, GotEmaggedEvent>(OnEmagged);
            SubscribeLocalEvent<BodyDissolverComponent, AfterInteractEvent>(OnAfterInteract);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            _queuedDestroyTacks.Clear();

            var query = EntityQueryEnumerator<BodyDissolverComponent>();

            while (query.MoveNext(out var uid, out var bodyDissolverComponent))
            {
                if (bodyDissolverComponent.SafetyEnabled ||
                    !TryComp<EmbeddableProjectileComponent>(uid, out var embeddableProjectileComponent) ||
                    embeddableProjectileComponent.Target is null)
                    continue;

                if (bodyDissolverComponent.DestroyBy < _timing.RealTime)
                {
                    _queuedDestroyTacks.Add(uid);
                }
            }

            foreach (var queued in _queuedDestroyTacks)
            {
                if (TryComp<TransformComponent>(queued, out var transformComponent))
                { // for some reason doing queued.ToCoordinates() makes the game stop playback immediately due to the entity being deleted.
                    _sharedAudioSystem.PlayPvs(new SoundCollectionSpecifier("GlassBreak"), transformComponent.Coordinates); // can the sound be not hardcoded? yeah. will anyone care? probably not.
                }
                QueueDel(queued);
            }
        }

        /// <summary>
        /// What this should do, step by step:
        /// 1. Checks if the dissolu-tee is a mob, has BodyDissolvableComponent (only if the safety is on) and is currently dead
        /// 2. Creates a puddle, plays a sound from the puddle.
        /// 3. Create a plume of gas based on the one in BodyDissolvableComponent
        /// 4. The contents of the puddle and its size are determined by the entity's bloodstream solution. This gets spilled.
        /// 5. Deletes both the tack and the entity that's getting dissolved
        /// </summary>
        private void OnEmbed(Entity<BodyDissolverComponent> tack, ref EmbedEvent args)
        {
            if (!tack.Comp.SafetyEnabled) // if the safety is disabled, that means we've added EmbedPassiveDamageComponent
            {
                tack.Comp.DestroyBy = _timing.RealTime + tack.Comp.EmaggedLifetime;
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-emagged"), InGameICChatType.Speak, hideChat: true);
                return;
            }

            if (!HasComp<BodyDissolvableComponent>(args.Embedded)) // check if the body has the relevant component
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dissolvable"), InGameICChatType.Speak, hideChat: true);
                return;
            }

            if (!_mobStateSystem.IsDead(args.Embedded)) // we only want this to work on dead mobs, otherwise uh well. nightmare!
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dead"), InGameICChatType.Speak, hideChat: true);
                return;
            }

            DissolveBody(tack, args.Embedded);

            if (tack.Comp.DestroyOnUse)
                Del(tack);
        }

        private void OnAfterInteract(Entity<BodyDissolverComponent> tack, ref AfterInteractEvent args)
        {
            if (args.Target is null)
                return;

            if (!tack.Comp.SafetyEnabled)
            {
                _sharedPopupSystem.PopupCursor(Loc.GetString("body-dissolution-throw"), args.User, PopupType.MediumCaution); // yes this is a hack. I just don't know how to properly embed something into someone when clicking on them
                return;
            }

            if (!HasComp<BodyDissolvableComponent>(args.Target))
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dissolvable"), InGameICChatType.Speak, hideChat: true);
                return;
            }

            if (!_mobStateSystem.IsDead((EntityUid) args.Target))
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dead"), InGameICChatType.Speak, hideChat: true);
                return;
            }

            DissolveBody(tack, (EntityUid) args.Target);
            if (tack.Comp.DestroyOnUse)
                Del(tack);
        }

        private void OnEmagged(Entity<BodyDissolverComponent> tack, ref GotEmaggedEvent args)
        {
            if (!tack.Comp.CanBeEmagged)
                return;

            _sharedAudioSystem.PlayPvs(tack.Comp.EmagSound, tack, AudioParams.Default.WithVolume(8));

            EnsureComp<EmaggedComponent>(tack);

            var embedPassiveDamageComponent = EnsureComp<EmbedPassiveDamageComponent>(tack);

            embedPassiveDamageComponent.Damage = tack.Comp.DamageWhenEmagged;

            tack.Comp.SafetyEnabled = false;
            args.Handled = true;
        }
        private void DissolveBody(Entity<BodyDissolverComponent> dissolver, EntityUid dissolutee)
        {
            if (!TryComp<BodyDissolvableComponent>(dissolutee, out var bodyDissolvableComponent) ||
                !TryComp<PhysicsComponent>(dissolutee, out var physicsComponent))
                return;

            _sharedSolutionContainerSystem.TryGetSolution(dissolutee, "bloodstream", out var _, out var bodyBloodstreamSolution);

            var solution = new Solution("Water", bodyDissolvableComponent.MaximumSpillAmount);
            var bloodColor = new Color(0, 255, 0, 255);

            if (bodyBloodstreamSolution is not null)
            {
                solution = bodyBloodstreamSolution.SplitSolution(Math.Min(bodyDissolvableComponent.MaximumSpillAmount, (float) bodyBloodstreamSolution.Volume));
                bloodColor = bodyBloodstreamSolution.GetColor(_proto).WithAlpha(255).WithGreen(255);
            }

            _puddleSystem.TrySpillAt(dissolutee.ToCoordinates(), solution, out var puddle, true);
            _sharedAudioSystem.PlayPvs(dissolver.Comp.DissolveSound, puddle);

            if (TryComp<BodyComponent>(dissolutee, out var bodyComponent))
            {
                _sharedAudioSystem.PlayPvs(bodyComponent.GibSound, puddle); // I could play the gib sound at the dissolu-tee's coordinates but this works just as well
            }

            var dissolveEffect = SpawnAtPosition(dissolver.Comp.DissolutionEffect, dissolutee.ToCoordinates());

            _paintSystem.Paint(null, null, dissolveEffect, bloodColor);

            var plume = new GasMixture(1) { Temperature = 330.0f };
            var environment = _atmosphereSystem.GetContainingMixture(dissolutee, true, true);

            if (environment is null)
            {
                Del(dissolutee);
                return;
            }

            plume.SetMoles(bodyDissolvableComponent.EmittedGas, physicsComponent.Mass * bodyDissolvableComponent.EmittedGasCoefficient);
            _atmosphereSystem.Merge(environment, plume);

            Del(dissolutee);
        }
    };
}
