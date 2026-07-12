// SPDX-FileCopyrightText: 2025 Falcon
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

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
using Content.Shared.DoAfter;
// Start TheDen - Add sounds to holofan
using Content.Shared.Sound;
using Content.Shared.Sound.Components;
using Robust.Shared.Serialization;


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
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!; // DEN: Sign placement doafters.
    [Dependency] private readonly SharedTransformSystem _xforms = default!; // DEN: Sign placement doafters.

    private HashSet<Entity<IComponent>> _signs = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChargeHolosignProjectorComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<ChargeHolosignProjectorComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ChargeHolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);

        SubscribeLocalEvent<ChargeHolosignProjectorComponent, SignPlaceActionEvent>(OnSignPlaceAction); // DEN: Place doafters
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
        {
            // DEN Start: Sign placement doafters.
            if (ent.Comp.PlaceTime > TimeSpan.Zero)
            {
                TryStartDoafter((ent, ent, charges), args);
            }
            else
            {
                TryPlaceSign(
                    (ent, ent, charges), 
                    args.User, 
                    _xforms.ToMapCoordinates(args.ClickLocation.SnapToGrid(EntityManager)));
            }
            // DEN End
        }
        else
            TryRemoveSign((ent, ent, charges), _signs.First(), args.User);

        args.Handled = true;
    }

    // DEN Start: Sign placement doafters
    public void TryStartDoafter(
        Entity<ChargeHolosignProjectorComponent, LimitedChargesComponent> ent, BeforeRangedInteractEvent args
    )
    {
        if (_charges.HasInsufficientCharges(ent.Owner, 1, ent.Comp2))
        {
            _popup.PopupClient(Loc.GetString("charge-holoprojector-no-charges", ("item", ent)), ent, args.User);
            return;
        }

        _doAfterSystem.TryStartDoAfter(
            new(
                    EntityManager,
                    args.User,
                    ent.Comp1.PlaceTime,
                    new SignPlaceActionEvent(_xforms.ToMapCoordinates(args.ClickLocation.SnapToGrid(EntityManager))),
                    ent.Owner,
                    used: ent.Owner)
            {
                BreakOnMove = true,
                BreakOnWeightlessMove = false
            });
    }

    public void OnSignPlaceAction(Entity<ChargeHolosignProjectorComponent> ent, ref SignPlaceActionEvent args)
    {
        if (args.Cancelled)
            return;
        
        if (!TryComp<LimitedChargesComponent>(ent, out var charges))
            return;

        TryPlaceSign((ent, ent.Comp, charges), args.User, args.Location);
    }
    // DEN End
    
    public bool TryPlaceSign(Entity<ChargeHolosignProjectorComponent, LimitedChargesComponent> ent, EntityUid user, MapCoordinates location) // DEN: Change arguments to not take ranged event.
    {
        if (!_charges.TryUseCharge((ent, ent.Comp2)))
        {
            _popup.PopupClient(Loc.GetString("charge-holoprojector-no-charges", ("item", ent)), ent, user);
            return false;
        }

        var holoUid = EntityManager.PredictedSpawn(ent.Comp1.SignProto, location);
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

// DEN: Sign placement doafter
[Serializable, NetSerializable]
public sealed partial class SignPlaceActionEvent : SimpleDoAfterEvent
{
    public MapCoordinates Location { get; set; }

    public SignPlaceActionEvent(MapCoordinates location) : this() => Location = location;
}
