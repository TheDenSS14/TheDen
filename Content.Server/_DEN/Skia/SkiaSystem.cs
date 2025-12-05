using Content.Server.Actions;
using Content.Server.Store.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Store.Components;

namespace Contnet.Server._DEN.Skia;

public sealed class SkiaSystem : EntitySystem
{
    internal const string SkiaShopId = "ActionSkiaShop";

    [Dependency] private readonly ActionsSystem _action = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedInteractionSystem _interact = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StoreSystem _store = default!;

    public override void Initialize(){
        base.Initialize();

        SubscribeLocalEvent<SkiaComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, SkiaComponent comp, MapInitEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        _action.AddAction(uid, SkiaShopId);
    }
}
