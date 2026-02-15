using Content.Server.Atmos.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Coordinates;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Content.Shared.Projectiles;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Content.Shared.Atmos;
using Content.Shared.Body.Components;
using Robust.Shared.Physics.Components;

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

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<BodyDissolverComponent, EmbedEvent>(OnEmbed);
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
            if (!HasComp<BodyDissolvableComponent>(args.Embedded))
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dissolvable"), InGameICChatType.Speak, hideChat: true);
                return;
            }
            /* // TODO: find a way to embed projectiles into mobs that are lying down
            if (!_mobStateSystem.IsDead(args.Embedded))
            {
                _sharedChatSystem.TrySendInGameICMessage(tack, Loc.GetString("body-dissolution-fail-not-dead"), InGameICChatType.Speak, true);
                return;
            }
            */

            DissolveBody(tack, args.Embedded);
            EntityManager.DeleteEntity(tack);
        }

        private void DissolveBody(Entity<BodyDissolverComponent> dissolver, EntityUid dissolutee)
        {
            if (!TryComp<BodyDissolvableComponent>(dissolutee, out var bodyDissolvableComponent) ||
                !TryComp<PhysicsComponent>(dissolutee, out var physicsComponent))
                return;

            _sharedSolutionContainerSystem.TryGetSolution(dissolutee, "bloodstream", out var _, out var bodyBloodstreamSolution);

            var solution = new Solution("Water", bodyDissolvableComponent.MaximumSpillAmount);

            if (bodyBloodstreamSolution is not null)
            {
                solution = bodyBloodstreamSolution.SplitSolution(Math.Min(bodyDissolvableComponent.MaximumSpillAmount, (float) bodyBloodstreamSolution.Volume));
            }

            _puddleSystem.TrySpillAt(dissolutee.ToCoordinates(), solution, out var puddle, true);
            _sharedAudioSystem.PlayPvs(dissolver.Comp.Sound, puddle);

            if (TryComp<BodyComponent>(dissolutee, out var bodyComponent))
            {
                _sharedAudioSystem.PlayPvs(bodyComponent.GibSound, puddle);
            }

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
