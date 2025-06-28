// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// Whenever an entity is inserted with silicon laws it will update the relevant entity's laws.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SiliconLawUpdaterComponent : Component
{
    /// <summary>
    /// Entities to update
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry Components;
}
