// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Salvage.Systems;
using Content.Shared.Thief;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
namespace Content.Shared._DV.Salvage.Components;

/// </summary>
///     A vendor that sells mining equipment. Also holds a radial menu to redeem vouchers with a preset list of items.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(MiningVoucherSystem))]
public sealed partial class MiningVendorComponent : Component
{
    /// <summary>
    /// The kits that can be selected.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<ThiefBackpackSetPrototype>> Kits = new();
}

