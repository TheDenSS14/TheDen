// SPDX-FileCopyrightText: 2024 Spatison
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Popups;
using Content.Shared.Hands.Components;
using Content.Shared.Alert;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.OfferItem;
using Content.Shared.IdentityManagement;
using Robust.Shared.Player;

namespace Content.Server.OfferItem;

// Floofstation - partial
public sealed partial class OfferItemSystem : SharedOfferItemSystem
{
    [Dependency] private readonly AlertsSystem _alertsSystem = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    // Floofstation
    public override void Initialize()
    {
        base.Initialize();
        InitializeTransfers();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<OfferItemComponent>();
        while (query.MoveNext(out var uid, out var offerItem))
        {
            if (!TryComp<HandsComponent>(uid, out var hands) || hands.ActiveHand == null)
                continue;

            if (offerItem.Hand != null &&
                hands.Hands[offerItem.Hand].HeldEntity == null)
            {
                if (offerItem.Target != null)
                {
                    UnReceive(offerItem.Target.Value, offerItem: offerItem);
                    offerItem.IsInOfferMode = false;
                    Dirty(uid, offerItem);
                }
                else
                    UnOffer(uid, offerItem);
            }

            if (!offerItem.IsInReceiveMode)
            {
                _alertsSystem.ClearAlert(uid, offerItem.OfferAlert);
                continue;
            }

            _alertsSystem.ShowAlert(uid, offerItem.OfferAlert);
        }
    }

    /// <summary>
    /// Accepting the offer and receive item
    /// </summary>
    public void Receive(EntityUid uid, OfferItemComponent? component = null)
    {
        // Floofstation note: (uid, component) is the receiver. (component.Target, offerItem) is the offerer. Why? I don't fucking know.
        if (!Resolve(uid, ref component) ||
            !TryComp<OfferItemComponent>(component.Target, out var offerItem) ||
            offerItem.Hand == null ||
            component.Target == null ||
            !TryComp<HandsComponent>(uid, out var hands))
            return;

        if (offerItem.Item != null)
        {
            // Floof - check if there's something else handling it first
            var realItem = offerItem.GetRealEntity(EntityManager);
            if (!TryHandleExtendedTransfer(component.Target.Value, uid, offerItem.Item.Value, realItem)
                && !_hands.TryPickup(uid, offerItem.Item.Value, handsComp: hands))
            {
                _popup.PopupEntity(Loc.GetString("offer-item-full-hand"), uid, uid);
                return;
            }

            _popup.PopupEntity(Loc.GetString("offer-item-give",
                ("item", Identity.Entity(realItem, EntityManager)), // FLoof - resolve virtual items
                ("target", Identity.Entity(uid, EntityManager))), component.Target.Value, component.Target.Value);
            _popup.PopupEntity(Loc.GetString("offer-item-give-other",
                    ("user", Identity.Entity(component.Target.Value, EntityManager)),
                    ("item", Identity.Entity(realItem, EntityManager)), // FLoof - resolve virtual items
                    ("target", Identity.Entity(uid, EntityManager)))
                , component.Target.Value, Filter.PvsExcept(component.Target.Value, entityManager: EntityManager), true);
        }

        offerItem.Item = null;
        UnReceive(uid, component, offerItem);
    }
}
