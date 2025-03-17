using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Robust.Shared.Network;


namespace Content.Shared.RpConsume;


public sealed class RpConsumeSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RpConsumeComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<RpConsumeComponent, RpConsumeDoAfterEvent>(OnDoAfter);
    }

    private void OnDoAfter(Entity<RpConsumeComponent> entity, ref RpConsumeDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (!_net.IsClient)
            QueueDel(entity);
        args.Handled = true;
    }

    private void OnUseInHand(Entity<RpConsumeComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        var msg = Loc.GetString(entity.Comp.ConsumePopup, ("name", Identity.Entity(entity, EntityManager)));
        _popupSystem.PopupPredicted(msg, args.User, args.User);

        var doAfterEventArgs = new DoAfterArgs(
            EntityManager,
            args.User,
            entity.Comp.Delay,
            new RpConsumeDoAfterEvent(),
            entity,
            entity,
            entity)
        {
            NeedHand = true,
            BreakOnMove = true,
            BreakOnWeightlessMove = false,
        };

        _doAfter.TryStartDoAfter(doAfterEventArgs);
        args.Handled = true;
    }
}
