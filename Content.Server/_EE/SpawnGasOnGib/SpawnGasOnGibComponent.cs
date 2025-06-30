// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos;

namespace Content.Server._EE.SpawnGasOnGib;

// <summary>
//   Spawns a gas mixture upon being gibbed.
// </summary>
[RegisterComponent]
public sealed partial class SpawnGasOnGibComponent : Component
{
    // <summary>
    //   The gas mixture to spawn.
    // </summary>
    [DataField("gasMixture", required: true)]
    public GasMixture Gas = new();
}
