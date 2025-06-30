// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Content.Shared.Humanoid;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryRemoveMarkingStepComponent : Component
{
    /// <summary>
    ///     The category the marking belongs to.
    /// </summary>
    [DataField]
    public HumanoidVisualLayers MarkingCategory = default!;

    /// <summary>
    ///     Can be either a segment of a marking ID, or an entire ID that will be checked
    ///     against the entity to validate that the marking is present.
    /// </summary>
    [DataField]
    public string MatchString = string.Empty;

    /// <summary>
    ///     Will this step spawn an item as a result of removing the markings? If so, which?
    /// </summary>
    [DataField]
    public EntProtoId? ItemSpawn = default!;
}
