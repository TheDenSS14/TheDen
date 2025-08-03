// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Shared.WhiteDream.BloodCult.Items.VoidTorch;

[RegisterComponent]
public sealed partial class VoidTorchComponent : Component
{
    [DataField]
    public int Charges = 5;

    [DataField]
    public SoundPathSpecifier TeleportSound = new("/Audio/_White/BloodCult/veilin.ogg");

    [DataField]
    public string TurnOnLightBehaviour = "turn_on";

    [DataField]
    public string TurnOffLightBehaviour = "fade_out";

    public EntityUid? TargetItem;
}
