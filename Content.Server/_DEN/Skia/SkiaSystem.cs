
using System.Numerics;
using Content.Server.Actions;
using Content.Server.GameTicking;
using Content.Server.Store.Components;
using Content.Server.Store.Systems;
using Content.Shared.Alert;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Eye;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Mobs.Systems;
using Content.Shared.Physics;
using Content.Shared.Popups;
using Content.Shared.Revenant;
using Content.Shared.Revenant.Components;
using Content.Shared._DEN.Skia.Components;
using Content.Shared.StatusEffect;
using Content.Shared.Store.Components;
using Content.Shared.Stunnable;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

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

        _action.AddAction(uid, ref Component.Action, SkiaShopId)
    }
}
