// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.WhiteDream.BloodCult.Runes.BloodBoil;

[RegisterComponent]
public sealed partial class CultRuneBloodBoilComponent : Component
{
    [DataField]
    public EntProtoId ProjectilePrototype = "BloodBoilProjectile";

    [DataField]
    public float ProjectileSpeed = 50;

    [DataField]
    public float TargetsLookupRange = 15f;

    [DataField]
    public float ProjectileCount = 3;

    [DataField]
    public float FireStacksPerProjectile = 1;

    [DataField]
    public SoundSpecifier ActivationSound = new SoundPathSpecifier("/Audio/_White/BloodCult/magic.ogg");
}
