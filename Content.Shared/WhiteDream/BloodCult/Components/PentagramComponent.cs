// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.WhiteDream.BloodCult.Components;

[NetworkedComponent, RegisterComponent]
public sealed partial class PentagramComponent : Component
{
    public ResPath RsiPath = new("/Textures/_White/BloodCult/Effects/pentagram.rsi");

    public readonly string[] States =
    [
        "halo1",
        "halo2",
        "halo3",
        "halo4",
        "halo5",
        "halo6"
    ];
}
