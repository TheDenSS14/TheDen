// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Physics;
using Content.Shared.StatusEffect;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.WhiteDream.BloodCult.Constructs.PhaseShift;

[RegisterComponent]
public sealed partial class PhaseShiftedComponent : Component
{
    [DataField]
    public ProtoId<StatusEffectPrototype> StatusEffectId = "PhaseShifted";

    [DataField]
    public float MovementSpeedBuff = 1.5f;

    [DataField]
    public int CollisionMask = (int) CollisionGroup.GhostImpassable;

    [DataField]
    public int CollisionLayer;

    [DataField]
    public EntProtoId PhaseInEffect = "EffectEmpPulseNoSound";

    [DataField]
    public EntProtoId PhaseOutEffect = "EffectEmpPulseNoSound";

    [DataField]
    public SoundSpecifier PhaseInSound = new SoundPathSpecifier(new ResPath("/Audio/_White/BloodCult/veilin.ogg"));

    [DataField]
    public SoundSpecifier PhaseOutSound =
        new SoundPathSpecifier(new ResPath("/Audio/_White/BloodCult/veilout.ogg"));

    public int StoredMask;
    public int StoredLayer;
}
