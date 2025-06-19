// SPDX-FileCopyrightText: 2024 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.OfferItem;
using Content.Server.OfferItem;
using Content.Shared.Alert;
using JetBrains.Annotations;

namespace Content.Server.Alert.Click;

/// <summary>
/// Accepting the offer and receive item
/// </summary>
[UsedImplicitly]
[DataDefinition]
public sealed partial class AcceptOffer : IAlertClick
{
    public void AlertClicked(EntityUid player)
    {
        var entManager = IoCManager.Resolve<IEntityManager>();

        if (entManager.TryGetComponent(player, out OfferItemComponent? offerItem))
        {
            entManager.System<OfferItemSystem>().Receive(player, offerItem);
        }
    }
}
