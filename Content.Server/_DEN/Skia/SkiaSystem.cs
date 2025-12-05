

using Content.Server.Store.Systems;
using Content.Shared._DEN.Skia;
using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Mobs;
using Content.Shared.Popups;
using Content.Shared.Stealth.Components;
using Content.Shared.Store.Components;

namespace Contnet.Server._DEN.Skia;

public sealed class SkiaSystem : SharedSkiaSystem
{

    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedInteractionSystem _interact = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StoreSystem _store = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkiaComponent, MobStateChangedEvent>(OnMobStageChanged);
        SubscribeLocalEvent<SkiaComponent, SkiaShopActionEvent>(OnShop);
    }


    private void OnShop(EntityUid uid, SkiaComponent comp, SkiaShopActionEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        _store.ToggleUi(uid, uid, store);
    }

    private void OnMobStageChanged(EntityUid uid, SkiaComponent comp, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
            RemComp<StealthComponent>(uid);

        if (args.NewMobState == MobState.Alive && !TryComp<StealthComponent>(uid, out var stealthComp))
        {
            AddComp<StealthComponent>(uid);
        }
    }
}
