// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Lavaland.Tendril.Components;

/// <summary>
/// A mob created by a tendril. Upon death, it is removed from its spawn list
/// </summary>
[RegisterComponent]
public sealed partial class TendrilMobComponent : Component
{
    public EntityUid? Tendril;
}
