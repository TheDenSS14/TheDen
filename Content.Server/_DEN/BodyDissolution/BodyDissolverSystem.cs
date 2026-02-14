using Content.Server.Fluids.EntitySystems;
using Content.Server.Speech.Components;
using Content.Shared.Chat;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Content.Shared.Projectiles;
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

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<BodyDissolverComponent, ProjectileEmbedEvent>(OnEmbed);
        }

        private void OnEmbed(Entity<BodyDissolverComponent> ent, ref ProjectileEmbedEvent args)
        {
            EntityManager.DeleteEntity(ent);
        }
    };
}
