using System.Linq;
using Content.Server._DEN.ItemModifiers.Components;
using Content.Shared.Destructible;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using ModifyOnApplyDoAfterEvent = Content.Shared._DEN.ItemModifiers.Events.ModifyOnApplyDoAfterEvent;


namespace Content.Server._DEN.ItemModifiers.Systems;


public sealed class ModifyOnApplySystem : EntitySystem
{
    [Dependency] private readonly IComponentFactory _componentFactory = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedDestructibleSystem _destructibleSystem = default!;
    [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ModifiableComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);
        SubscribeLocalEvent<ModifyOnApplyComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<ModifyOnApplyComponent, ModifyOnApplyDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<ModifyOnApplyComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is null || !args.CanReach)
            return;

        if(TryStartApplyDoafter(ent, args.Target.Value, args.User))
            args.Handled = true;
    }

    private void OnDoAfter(EntityUid uid, ModifyOnApplyComponent modifyOnApplyComp, DoAfterEvent args)
    {
        if (args.Handled || args.Args.Target is null ||
            !TryComp<ModifiableComponent>(args.Args.Target, out var modifiableComp))
            return;

        if (args.Cancelled)
            return;

        var target = args.Args.Target.Value;
        var user = args.Args.User;

        if (!CheckEntityRequirements(uid, target))
        {
            _popupSystem.PopupEntity(Loc.GetString(modifyOnApplyComp.InvalidMessage, ("target", target), ("source", uid)), user);
            args.Handled = true;
            return;
        }

        if (modifyOnApplyComp.RemoveComps is not null)
            foreach (var comp in modifyOnApplyComp.RemoveComps)
                RemComp(target, _componentFactory.GetComponent(comp).GetType());

        EntityManager.AddComponents(target, modifyOnApplyComp.ApplyComps, true);

        _popupSystem.PopupEntity(Loc.GetString(modifyOnApplyComp.PostApplyMessage, ("target", target), ("source", uid)), user);

        if(modifyOnApplyComp.ModifyName is not null)
            _metaDataSystem.SetEntityName(target, Loc.GetString(modifyOnApplyComp.ModifyName, ("target", Name(target))));

        if(modifyOnApplyComp.ModifyDescription is not null)
            _metaDataSystem.SetEntityDescription(target, Description(target) + "\n" + Loc.GetString(modifyOnApplyComp.ModifyDescription));

        DirtyEntity(target);

        _destructibleSystem.DestroyEntity(uid);

        args.Handled = true;
    }

    private bool TryStartApplyDoafter(EntityUid source, EntityUid target, EntityUid user)
    {
        if (!TryComp<ModifyOnApplyComponent>(source, out var modifyOnApplyComp))
            return false;

        if (!CheckEntityRequirements(source, target))
        {
            _popupSystem.PopupEntity(Loc.GetString(modifyOnApplyComp.InvalidMessage, ("target", target), ("source", source)), target, user);
            return false;
        }

        var doAfter =
            new DoAfterArgs(
                    EntityManager,
                    user,
                    modifyOnApplyComp.UseTime,
                    new ModifyOnApplyDoAfterEvent(),
                    source,
                    target: target,
                    used: source)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                NeedHand = true,
            };
        _doAfterSystem.TryStartDoAfter(doAfter);
        return true;
    }

    private bool CheckEntityRequirements(EntityUid source, EntityUid target)
    {
        if (!TryComp<ModifyOnApplyComponent>(source, out var modifyOnApplyComp) ||
            !TryComp<ModifiableComponent>(target, out var modifiableComp))
            return false;

        if (modifyOnApplyComp.WhitelistTags is not null &&
                !modifyOnApplyComp.WhitelistTags.IsSubsetOf(modifiableComp.Tags))
        {
            return false;
        }

        if (modifyOnApplyComp.BlacklistTags is not null &&
                !modifyOnApplyComp.BlacklistTags.Overlaps(modifiableComp.Tags))
        {
            return false;
        }

        if (modifyOnApplyComp.WhitelistComps is not null)
        {
            if (modifyOnApplyComp.WhitelistComps.Any(comp => !HasComp(target, _componentFactory.GetComponent(comp).GetType())))
            {
                return false;
            }
        }

        return modifyOnApplyComp.BlacklistComps is null ||
            modifyOnApplyComp.BlacklistComps.All(comp => !HasComp(target, _componentFactory.GetComponent(comp).GetType()));
    }

    private void OnGetInteractionVerbs(Entity<ModifiableComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (args.Hands == null || !args.CanAccess || !args.CanInteract)
            return;

        if (!args.Using.HasValue)
            return;

        if (!CheckEntityRequirements(args.Using!.Value, ent))
            return;

        var source = args.Using!.Value;
        var target = args.Target;
        var user = args.User;

        if (!TryComp<ModifyOnApplyComponent>(source, out var modifyOnApplyComp))
            return;

        InteractionVerb verb = new()
        {
            Act = () =>
            {
                TryStartApplyDoafter(source, target, user);
            },
            Text = Loc.GetString(modifyOnApplyComp.ApplyVerb, ("source", source)),
        };

        args.Verbs.Add(verb);
    }
}
