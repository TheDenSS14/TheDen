using System.Linq;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Speech.Components;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Coordinates;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Content.Shared.Projectiles;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server.BodyDissolution
{
    public sealed class BodyDissolutionSystem : EntitySystem
    {
        [Dependency] private readonly SharedAudioSystem _sharedAudioSystem = default!;
        [Dependency] private readonly PuddleSystem _puddleSystem = default!;
        [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
        [Dependency] private readonly SharedMindSystem _minds = default!;
        [Dependency] private readonly SharedChatSystem _sharedChatSystem = default!;
        [Dependency] private readonly SharedSolutionContainerSystem _sharedSolutionContainerSystem = default!;

        public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/_DEN/Effects/body_dissolver_tack.ogg");

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<BodyDissolverComponent, EmbedEvent>(OnEmbed);
        }

        /// <summary>
        /// Checks if the dissolu-tee is a mob, has BodyDissolvableComponent (only if the safety is on) and is currently dead
        /// Creates a puddle, plays a sound from the puddle.
        /// The contents of the puddle and its size are determined by the entity's bloodstream solution. This gets turned into goo and spilled.
        /// Deletes both the tack and the entity that's getting dissolved
        /// </summary>
        private void OnEmbed(Entity<BodyDissolverComponent> ent, ref EmbedEvent args)
        {
            _sharedSolutionContainerSystem.TryGetSolution(args.Embedded, "bloodstream", out var solutionCompEnt, out var bodyBloodstreamSolution);

            if (bodyBloodstreamSolution is null)
            {
                var solution = new Solution("Water", 10);
                _puddleSystem.TrySpillAt(args.Embedded.ToCoordinates(), solution, out var puddle, false);
                _sharedAudioSystem.PlayPvs(Sound, puddle);
            }
            else
            {
                _puddleSystem.TrySpillAt(args.Embedded.ToCoordinates(), bodyBloodstreamSolution, out var puddle, false);
                _sharedAudioSystem.PlayPvs(Sound, puddle);
            }

            EntityManager.DeleteEntity(args.Embedded);
            EntityManager.DeleteEntity(ent);
        }
    };
}
