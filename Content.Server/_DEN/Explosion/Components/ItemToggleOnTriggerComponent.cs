// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server._DEN.Explosion.Components;

/// <summary>
/// Makes an ItemToggle toggle occur when a trigger is received.
/// </summary>
[RegisterComponent]
public sealed partial class ItemToggleOnTriggerComponent : Component
{
    /// <summary>
    /// Can the item be toggled on using the trigger?
    /// </summary>
    [DataField] public bool CanActivate = true;
    
    /// <summary>
    /// Can the item be toggled off using the trigger?
    /// </summary>
    [DataField] public bool CanDeactivate = true;
}
