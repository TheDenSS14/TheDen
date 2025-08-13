// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using System.Linq;
// Start TheDen - Add sounds to holofan
using Content.Shared.Sound;
using Content.Shared.Sound.Components;
// End TheDen

namespace Content.Shared._DV.Holosign;

public sealed class ChargeHolosignSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedChargesSystem _charges = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedEmitSoundSystem _sound = default!;

    private HashSet<Entity<IComponent>> _signs = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChargeHolosignProjectorComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<ChargeHolosignProjectorComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ChargeHolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);
    }

    private void OnInit(Entity<ChargeHolosignProjectorComponent> ent, ref ComponentInit args)
    {
        // its required, funny test is still funny
        if (string.IsNullOrEmpty(ent.Comp.SignComponentName))
            return;

        ent.Comp.SignComponent = EntityManager.ComponentFactory.GetRegistration(ent.Comp.SignComponentName).Type;
    }

    private void OnMapInit(Entity<ChargeHolosignProjectorComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<LimitedChargesComponent>(ent, out var charges))
            return;
    }

    private void OnBeforeInteract(Entity<ChargeHolosignProjectorComponent> ent, ref BeforeRangedInteractEvent args)
    {
        if (args.Handled || !args.CanReach ||
            HasComp<StorageComponent>(args.Target) || // if it's a storage component like a bag, we ignore usage so it can be stored
            !TryComp<LimitedChargesComponent>(ent, out var charges))
            return;

        // first check if there's any existing holofans to clear
        var coords = args.ClickLocation.SnapToGrid(EntityManager);
        var mapCoords = _transform.ToMapCoordinates(coords);

        _signs.Clear();

        _lookup.GetEntitiesInRange(ent.Comp.SignComponent, mapCoords, 0.25f, _signs);

        if (_signs.Count == 0)
            TryPlaceSign((ent, ent, charges), args);
        else
            TryRemoveSign((ent, ent, charges), _signs.First(), args.User);

        args.Handled = true;
    }

    public bool TryPlaceSign(Entity<ChargeHolosignProjectorComponent, LimitedChargesComponent> ent, BeforeRangedInteractEvent args)
    {
        if (!_charges.TryUseCharge((ent, ent.Comp2)))
        {
            _popup.PopupClient(Loc.GetString("charge-holoprojector-no-charges", ("item", ent)), ent, args.User);
            return false;
        }

        var holoUid = EntityManager.PredictedSpawnAtPosition(ent.Comp1.SignProto, args.ClickLocation.SnapToGrid(EntityManager));
        var xform = Transform(holoUid);
        if (!xform.Anchored)
            _transform.AnchorEntity(holoUid, xform); // anchor to prevent any tempering with (don't know what could even interact with it)

        return true;
    }

    public bool TryRemoveSign(Entity<ChargeHolosignProjectorComponent, LimitedChargesComponent> ent, EntityUid sign, EntityUid user)
    {
        // don't overfill
        if (ent.Comp2.Charges < ent.Comp2.MaxCharges)
        {
            _charges.AddCharges(ent, 1, ent);
        }

        var userIdentity = Identity.Name(user, EntityManager);
        _popup.PopupPredicted(
            Loc.GetString("charge-holoprojector-reclaim", ("sign", sign)),
            Loc.GetString("charge-holoprojector-reclaim-others", ("sign", sign), ("user", userIdentity)),
            ent,
            user);

        EntityManager.PredictedDeleteEntity(sign);
        return true;
    }
}
