using Content.Server._DEN.BluespacePlushiePatch.Components;
using Content.Server.Nutrition.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Destructible;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nyanotrasen.Item.PseudoItem;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Tools.Components;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using BluespacePlushiePatchableComponent = Content.Server._DEN.BluespacePlushiePatch.Components.BluespacePlushiePatchableComponent;
using BluespacePlushiePatchDoAfterEvent = Content.Shared._DEN.BluespacePlushiePatch.Events.BluespacePlushiePatchDoAfterEvent;


namespace Content.Server._DEN.BluespacePlushiePatch.Systems;


public sealed class BluespacePlushiePatchSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedDestructibleSystem _destructibleSystem = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _interfaceSystem = default!;
    [Dependency] private readonly SharedStorageSystem _storageSystem = default!;
    [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BluespacePlushiePatchableComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);
        SubscribeLocalEvent<BluespacePlushiePatchableComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<BluespacePlushiePatchComponent, AfterInteractEvent>(OnAfterInteract);

        SubscribeLocalEvent<BluespacePlushiePatchComponent, BluespacePlushiePatchDoAfterEvent>(OnDoAfter);
    }

    private void OnExamine(EntityUid uid, BluespacePlushiePatchableComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        if (component.HasPatch)
        {
            var name = Identity.Name(uid, EntityManager);
            args.PushMarkup(Loc.GetString("bluespace-plushie-patch-examine", ("name", name)));
        }
    }

    private void OnAfterInteract(Entity<BluespacePlushiePatchComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is null || !args.CanReach)
            return;

        if (TryStartApplyPatchDoafter(ent, args.Target.Value, args.User))
            args.Handled = true;
    }

    private void OnDoAfter(EntityUid uid, BluespacePlushiePatchComponent patchComp, DoAfterEvent args)
    {
        if (args.Handled || args.Args.Target is null ||
            !TryComp<BluespacePlushiePatchableComponent>(args.Args.Target, out var patchableComp))
            return;

        if (args.Cancelled)
            return;

        var target = args.Args.Target.Value;
        var user = args.Args.User;

        if (patchableComp.HasPatch)
        {
            _popupSystem.PopupEntity(Loc.GetString("bluespace-plushie-patch-already-has", ("target", target)), user);
            return;
        }

        foreach (var cont in _containerSystem.GetAllContainers(target))
        {
            _containerSystem.EmptyContainer(cont, true);
        }

        // This is bad and terrible and makes this system only apply to plushies and I'm sorry
        RemComp<ContainerManagerComponent>(target);
        RemComp<SecretStashComponent>(target);
        RemComp<SolutionContainerManagerComponent>(target);
        RemComp<FoodComponent>(target);
        RemComp<FoodSequenceElementComponent>(target); // No putting people in burgers.
        RemComp<ToolOpenableComponent>(target);

        EnsureComp<ContainerManagerComponent>(target);
        var container = _containerSystem.EnsureContainer<Container>(target, "storagebase");

        var storage = EnsureComp<StorageComponent>(target);
        EnsureComp<UserInterfaceComponent>(target);
        EnsureComp<AllowsSleepInsideComponent>(target);

        storage.Container = container;
        storage.Grid = new() { new(0, 0, 5, 4) };
        _storageSystem.SetMaxItemSize(target, _proto.Index<ItemSizePrototype>("Huge"));

        _interfaceSystem.SetUi(
            target,
            StorageComponent.StorageUiKey.Key,
            new InterfaceData("StorageBoundUserInterface"));

        _popupSystem.PopupEntity(Loc.GetString("bluespace-plushie-patch-applied", ("target", target)), user);
        _metaDataSystem.SetEntityName(target, "bluespace " + Identity.Name(target, EntityManager));

        DirtyEntity(target);

        patchableComp.HasPatch = true;

        _destructibleSystem.DestroyEntity(uid);

        args.Handled = true;
    }

    private bool TryStartApplyPatchDoafter(EntityUid patch, EntityUid target, EntityUid user)
    {
        if (!TryComp<BluespacePlushiePatchComponent>(patch, out var patchComp))
            return false;

        if (!TryComp<BluespacePlushiePatchableComponent>(target, out var patchableComp))
        {
            _popupSystem.PopupEntity(Loc.GetString("bluespace-plushie-patch-not-applicable", ("target", target)), user);
            return false;
        }

        if (patchableComp.HasPatch)
        {
            _popupSystem.PopupEntity(Loc.GetString("bluespace-plushie-patch-already-has", ("target", target)), user);
            return false;
        }

        var doAfter =
            new DoAfterArgs(
                EntityManager,
                user,
                patchComp.PatchUseTime,
                new BluespacePlushiePatchDoAfterEvent(),
                patch,
                target: target,
                used: patch)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                NeedHand = true,
            };
        _doAfterSystem.TryStartDoAfter(doAfter);
        return true;
    }

    private void OnGetInteractionVerbs(Entity<BluespacePlushiePatchableComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (args.Hands == null || !args.CanAccess || !args.CanInteract)
            return;

        if (!HasComp<BluespacePlushiePatchComponent>(args.Using))
            return;

        // Make the lambda happy with the refs
        var patch = args.Using!.Value;
        var target = args.Target;
        var user = args.User;

        InteractionVerb verb = new()
        {
            Act = () =>
            {
                TryStartApplyPatchDoafter(patch, target, user);
            },
            Text = Loc.GetString("bluespace-plushie-patch-verb-name"),
        };

        args.Verbs.Add(verb);
    }
}
