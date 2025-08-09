using Content.Server._DEN.Bed.Components;
using Content.Server.Actions;
using Content.Shared.Bed.Sleep;
using Content.Shared.Buckle.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Timing;
using Robust.Shared.Utility;


namespace Content.Server._DEN.Bed.Systems
{
    public sealed class StabilizeOnBuckleSystem : EntitySystem
    {
        [Dependency] private readonly ActionsSystem _actionsSystem = default!;
        [Dependency] private readonly SleepingSystem _sleepingSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StabilizeOnBuckleComponent, StrappedEvent>(OnStrapped);
            SubscribeLocalEvent<StabilizeOnBuckleComponent, UnstrappedEvent>(OnUnstrapped);
        }
        private void OnStrapped(Entity<StabilizeOnBuckleComponent> bed, ref StrappedEvent args)
        {
            _actionsSystem.AddAction(args.Buckle, ref bed.Comp.SleepAction, SleepingSystem.SleepActionId, bed);

            // Single action entity, cannot strap multiple entities to the same rollerbed.
            DebugTools.AssertEqual(args.Strap.Comp.BuckledEntities.Count, 1);
        }

        private void OnUnstrapped(Entity<StabilizeOnBuckleComponent> bed, ref UnstrappedEvent args)
        {
            _actionsSystem.RemoveAction(args.Buckle, bed.Comp.SleepAction);
            _sleepingSystem.TryWaking(args.Buckle.Owner);
        }
    }
}
