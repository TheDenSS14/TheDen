// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Actions.Events;

/// <summary>
/// Adds / removes the component upon action.
/// </summary>
[Virtual]
public partial class ActionComponentChangeEvent : InstantActionEvent
{
    [DataField(required: true)]
    public ComponentRegistry Components = new();
}

/// <summary>
/// Similar to <see cref="ActionComponentChangeEvent"/> except raises an event to attempt to relay it.
/// </summary>
public sealed partial class RelayedActionComponentChangeEvent : ActionComponentChangeEvent
{

}

[ByRefEvent]
public record struct AttemptRelayActionComponentChangeEvent
{
    public EntityUid? Target;
}
