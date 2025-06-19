// SPDX-FileCopyrightText: 2025 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.WhiteDream.BloodCult.Items.BaseAura;
using Robust.Shared.Prototypes;

namespace Content.Shared.WhiteDream.BloodCult.Items.ShadowShacklesAura;

[RegisterComponent]
public sealed partial class ShadowShacklesAuraComponent : BaseAuraComponent
{
    [DataField]
    public EntProtoId ShacklesProto = "ShadowShackles";

    [DataField]
    public TimeSpan MuteDuration = TimeSpan.FromSeconds(5);

    [DataField]
    public float DistanceThreshold = 1.5f;

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid Shackles;

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid Target;
}
