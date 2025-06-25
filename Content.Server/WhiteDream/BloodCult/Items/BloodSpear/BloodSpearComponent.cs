// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.WhiteDream.BloodCult.Items.BloodSpear;

[RegisterComponent]
public sealed partial class BloodSpearComponent : Component
{
    [DataField]
    public EntityUid? Master;

    [DataField]
    public TimeSpan ParalyzeTime = TimeSpan.FromSeconds(4);

    [DataField]
    public EntProtoId RecallActionId = "ActionBloodSpearRecall";

    public EntityUid? RecallAction;

    [DataField]
    public SoundSpecifier RecallAudio = new SoundPathSpecifier(
        new ResPath("/Audio/_White/BloodCult/rites.ogg"),
        AudioParams.Default.WithVolume(-3));
}
