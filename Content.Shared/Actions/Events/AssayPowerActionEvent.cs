// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Shared.Actions.Events;

public sealed partial class AssayPowerActionEvent : EntityTargetActionEvent
{
    [DataField]
    public TimeSpan UseDelay = TimeSpan.FromSeconds(8f);

    [DataField]
    public SoundSpecifier SoundUse = new SoundPathSpecifier("/Audio/Psionics/heartbeat_fast.ogg");

    [DataField]
    public string PopupTarget = "assay-begin";

    [DataField]
    public int FontSize = 12;

    [DataField]
    public string FontColor = "#8A00C2";

    [DataField]
    public int MinGlimmer = 3;

    [DataField]
    public int MaxGlimmer = 6;

    [DataField]
    public string PowerName = "assay";
}
