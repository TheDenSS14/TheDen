// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Floofstation.Leash.Components;

[RegisterComponent]
public sealed partial class LeashedComponent : Component
{
    public const string VisualsContainerName = "leashed-visuals";

    [DataField]
    public string? JointId = null;

    [NonSerialized]
    public EntityUid? Puller = null, Anchor = null;
}
