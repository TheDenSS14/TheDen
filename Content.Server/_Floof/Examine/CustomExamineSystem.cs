using Content.Shared._Floof.Examine;


namespace Content.Server._Floof.Examine;


public sealed class CustomExamineSystem : SharedCustomExamineSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<SetCustomExamineMessage>(OnSetCustomExamineMessage);
    }

    private void OnSetCustomExamineMessage(SetCustomExamineMessage msg, EntitySessionEventArgs args)
    {
        var target = GetEntity(msg.Target);
        if (!CanChangeExamine(args.SenderSession, target))
            return;

        var comp = EnsureComp<CustomExamineComponent>(target);

        TrimData(ref msg.PublicData, ref msg.SubtleData);
        comp.PublicData = msg.PublicData;
        comp.SubtleData = msg.SubtleData;

        Dirty(target, comp);
    }
}
