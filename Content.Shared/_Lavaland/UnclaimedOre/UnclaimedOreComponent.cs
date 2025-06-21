// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._Lavaland.UnclaimedOre;

/// <summary>
///     Component that holds information about ore that hasn't been processed yet.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class UnclaimedOreComponent : Component
{
    [DataField]
    public float MiningPoints;
}

