// SPDX-FileCopyrightText: 2025 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;


namespace Content.Shared.FootPrint;

// Floof: this system has been effectively rewriteen. DO NOT MERGE UPSTREAM CHANGES.
[RegisterComponent]
public sealed partial class PuddleFootPrintsComponent : Component
{
    /// <summary>
    ///     Ratio between puddle volume and the amount of reagents that can be transferred from it.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 SizeRatio = 0.75f;
}
