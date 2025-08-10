using Content.Shared._DEN.Earmuffs;

namespace Content.Server._DEN.Earmuffs;


public sealed class EarmuffsSystem : SharedEarmuffsSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<EarmuffsUpdated>(OnEarmuffsUpdated);
    }

    private void OnEarmuffsUpdated(EarmuffsUpdated msg, EntitySessionEventArgs args)
    {
        if (args.SenderSession.AttachedEntity is not { Valid: true } attachedEntity)
            return;

        var earmuffs = EnsureComp<EarmuffsComponent>(attachedEntity);
        earmuffs.HearRange = msg.HearRangePercentage;
    }
}
