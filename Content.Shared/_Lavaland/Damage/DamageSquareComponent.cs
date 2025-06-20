// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Shared._Lavaland.Damage;

[RegisterComponent]
public sealed partial class DamageSquareComponent : Component
{
    /// <summary>
    /// Entity that caused this damaging square to spawn.
    /// It will be ignored by this square.
    /// </summary>
    [DataField]
    public EntityUid OwnerEntity;

    [DataField(required: true)]
    public DamageSpecifier Damage = new();

    [DataField]
    public SoundPathSpecifier? Sound;

    /// <summary>
    /// After how many seconds we should deal the damage to all entities above.
    /// 0.1 by default because ping will make it unfair
    /// </summary>
    [DataField]
    public float DamageDelay = 0.2f;
}
