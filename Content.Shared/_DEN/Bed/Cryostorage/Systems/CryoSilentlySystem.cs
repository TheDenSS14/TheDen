using Content.Shared._DEN.Bed.Cryostorage.Components;
using Content.Shared.ActionBlocker;
using Content.Shared.Containers;
using Content.Shared.Verbs;
using Robust.Shared.Containers;


namespace Content.Shared._DEN.Bed.Cryostorage.Systems;

public sealed class CryoSilentlySystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly DragInsertContainerSystem _dragInsertContainer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryostorageSilentlyComponent, GetVerbsEvent<AlternativeVerb>>(OnGetAlternativeVerb);
    }

    // This function is almost the same as the DragInsertContainer system,
    // it just sets the user to be silently inserted first.
    private void OnGetAlternativeVerb(Entity<CryostorageSilentlyComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        var (uid, comp) = ent;

        // If we're on an entity that doesn't have a DragInsertContainerComponent something is wrong.
        if (!TryComp<DragInsertContainerComponent>(uid, out var dragInsert))
            return;

        if (!args.CanInteract || !args.CanAccess || args.Hands == null)
            return;

        if (!_container.TryGetContainer(uid, comp.ContainerId, out var container))
            return;

        var user = args.User;
        if (!_actionBlocker.CanInteract(user, ent))
            return;

        // Self-insert silently verb.
        if (_container.CanInsert(user, container) &&
            _actionBlocker.CanMove(user))
        {
            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    AddComp(user, new CryoingSilentlyComponent());
                    _dragInsertContainer.Insert(user, user, ent, container);
                },
                Text = Loc.GetString("cryo-silently-verb-text"),
                Message = Loc.GetString("cryo-silently-verb-message"),
                Priority = 1
            };
            args.Verbs.Add(verb);
        }
    }
}
