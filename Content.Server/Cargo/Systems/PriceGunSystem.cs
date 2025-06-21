// SPDX-FileCopyrightText: 2022 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 rolfero <45628623+rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 AJCM-git <60196617+ajcm-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 blueDev2 <89804215+blueDev2@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ike709 <ike709@github.com>
// SPDX-FileCopyrightText: 2024 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Cargo.Components;
using Content.Server.Popups;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Timing;
using Content.Shared.Verbs;

namespace Content.Server.Cargo.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class PriceGunSystem : EntitySystem
{
    [Dependency] private readonly UseDelaySystem _useDelay = default!;
    [Dependency] private readonly PricingSystem _pricingSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly CargoSystem _bountySystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<PriceGunComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<PriceGunComponent, GetVerbsEvent<UtilityVerb>>(OnUtilityVerb);
    }

    private void OnUtilityVerb(EntityUid uid, PriceGunComponent component, GetVerbsEvent<UtilityVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Using == null)
            return;

        if (!TryComp(uid, out UseDelayComponent? useDelay) || _useDelay.IsDelayed((uid, useDelay)))
            return;

        var price = _pricingSystem.GetPrice(args.Target);

        var verb = new UtilityVerb()
        {
            Act = () =>
            {
                _popupSystem.PopupEntity(Loc.GetString("price-gun-pricing-result", ("object", Identity.Entity(args.Target, EntityManager)), ("price", $"{price:F2}")), args.User, args.User);
                _useDelay.TryResetDelay((uid, useDelay));
            },
            Text = Loc.GetString("price-gun-verb-text"),
            Message = Loc.GetString("price-gun-verb-message", ("object", Identity.Entity(args.Target, EntityManager)))
        };

        args.Verbs.Add(verb);
    }

    private void OnAfterInteract(EntityUid uid, PriceGunComponent component, AfterInteractEvent args)
    {
        if (!args.CanReach || args.Target == null || args.Handled)
            return;

        if (!TryComp(uid, out UseDelayComponent? useDelay) || _useDelay.IsDelayed((uid, useDelay)))
            return;

        // Check if we're scanning a bounty crate
        if (_bountySystem.IsBountyComplete(args.Target.Value, out _))
        {
            _popupSystem.PopupEntity(Loc.GetString("price-gun-bounty-complete"), args.User, args.User);
        }
        else // Otherwise appraise the price
        {
            double price = _pricingSystem.GetPrice(args.Target.Value);
            _popupSystem.PopupEntity(Loc.GetString("price-gun-pricing-result", ("object", Identity.Entity(args.Target.Value, EntityManager)), ("price", $"{price:F2}")), args.User, args.User);
        }

        _useDelay.TryResetDelay((uid, useDelay));
        args.Handled = true;
    }
}
