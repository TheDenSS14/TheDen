using Content.Shared.Store.Components;
using Content.Shared.Actions;

namespace Content.Shared._DEN.Skia;

public abstract class SharedSkiaSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _action = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkiaComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, SkiaComponent comp, MapInitEvent args)
    {
        _action.AddAction(uid, ref comp.ShopAction, comp.ShopActionId);
    }
}

public sealed partial class SkiaShopActionEvent : InstantActionEvent { }
