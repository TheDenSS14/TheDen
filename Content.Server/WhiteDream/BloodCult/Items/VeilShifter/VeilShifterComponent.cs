// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Server.WhiteDream.BloodCult.Items.VeilShifter;

[RegisterComponent]
public sealed partial class VeilShifterComponent : Component
{
    [DataField]
    public int Charges = 4;

    [DataField]
    public int TeleportDistanceMax = 10;

    [DataField]
    public int TeleportDistanceMin = 5;

    [DataField]
    public Vector2i Offset = Vector2i.One * 2;

    // How many times it will try to find safe location before aborting the operation?
    [DataField]
    public int Attempts = 10;

    [DataField]
    public SoundPathSpecifier TeleportInSound = new("/Audio/_White/BloodCult/veilin.ogg");

    [DataField]
    public SoundPathSpecifier TeleportOutSound = new("/Audio/_White/BloodCult/veilout.ogg");

    [ViewVariables(VVAccess.ReadOnly), DataField("teleportInEffect")]
    public string? TeleportInEffect = "CultTeleportInEffect";

    [ViewVariables(VVAccess.ReadOnly), DataField("teleportOutEffect")]
    public string? TeleportOutEffect = "CultTeleportOutEffect";
}
