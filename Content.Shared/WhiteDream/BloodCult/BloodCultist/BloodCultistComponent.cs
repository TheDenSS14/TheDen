// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using Content.Shared.Antag;
using Content.Shared.FixedPoint;
using Content.Shared.Language;
using Content.Shared.Mind;
using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.WhiteDream.BloodCult.BloodCultist;

[RegisterComponent, NetworkedComponent]
public sealed partial class BloodCultistComponent : Component
{
    [DataField]
    public float HolyConvertTime = 15f;

    [DataField]
    public int MaximumAllowedEmpowers = 4;

    [DataField]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "BloodCultMember";

    [DataField]
    public bool IconVisibleToGhost { get; set; } = true;

    [DataField]
    public ProtoId<LanguagePrototype> CultLanguageId { get; set; } = "Eldritch";

    [ViewVariables, NonSerialized]
    public EntityUid? BloodSpear;

    [ViewVariables, NonSerialized]
    public Entity<MindComponent>? OriginalMind;

    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 RitesBloodAmount = FixedPoint2.Zero;

    public Color OriginalEyeColor = Color.White;

    public CancellationTokenSource? DeconvertToken { get; set; }
}
