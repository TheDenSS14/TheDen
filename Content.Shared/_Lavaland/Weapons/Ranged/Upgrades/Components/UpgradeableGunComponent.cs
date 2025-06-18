// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._Lavaland.Weapons.Ranged.Upgrades.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedGunUpgradeSystem))]
public sealed partial class UpgradeableGunComponent : Component
{
    [DataField]
    public string UpgradesContainerId = "upgrades";

    [DataField]
    public EntityWhitelist Whitelist = new();

    [DataField]
    public int? MaxUpgradeCount;

    [DataField]
    public int MaxUpgradeCapacity = 100;
}
