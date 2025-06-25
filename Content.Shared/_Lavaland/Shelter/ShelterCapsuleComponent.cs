// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.GridPreloader.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared._Lavaland.Shelter;

[RegisterComponent]
public sealed partial class ShelterCapsuleComponent : Component
{
    [DataField]
    public float DeployTime = 1f;

    [DataField(required: true)]
    public ProtoId<PreloadedGridPrototype> PreloadedGrid;

    [DataField(required: true)]
    public Vector2 BoxSize;

    /// <remarks>
    /// This is needed only to fix the grid. Capsule always should spawn
    /// at the center, and this vector is required to ensure that.
    /// </remarks>>
    [DataField]
    public Vector2 Offset;
}
