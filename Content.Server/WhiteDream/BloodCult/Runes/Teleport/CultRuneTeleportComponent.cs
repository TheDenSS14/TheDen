// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Server.WhiteDream.BloodCult.Runes.Teleport;

[RegisterComponent]
public sealed partial class CultRuneTeleportComponent : Component
{
    [DataField]
    public float TeleportGatherRange = 0.65f;

    [DataField]
    public string Name = "";

    [DataField]
    public SoundPathSpecifier TeleportInSound = new("/Audio/_White/BloodCult/veilin.ogg");

    [DataField]
    public SoundPathSpecifier TeleportOutSound = new("/Audio/_White/BloodCult/veilout.ogg");
}
