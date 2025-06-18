// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Projectiles.SpawnEntitiesOnHit;

[RegisterComponent]
public sealed partial class SpawnEntitiesOnHitComponent : Component
{
    /// <summary>
    /// The prototype ID of the entity to spawn on hit
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Proto;

    /// <summary>
    /// The number of entities to spawn when the projectile hits
    /// </summary>
    [DataField]
    public int Amount = 1;
}
