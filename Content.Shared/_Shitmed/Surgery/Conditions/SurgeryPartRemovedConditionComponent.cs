// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Body.Part;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryPartRemovedConditionComponent : Component
{
    /// <summary>
    ///     Requires that the parent part can attach a new part to this slot.
    /// </summary>
    [DataField(required: true)]
    public string Connection = string.Empty;

    [DataField]
    public BodyPartType Part;

    [DataField]
    public BodyPartSymmetry? Symmetry;
}
