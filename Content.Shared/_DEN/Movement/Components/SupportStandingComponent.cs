// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared._DEN.Movement.Components;

/// <summary>
///     A component given to items that allow characters with missing legs to stand up instead of being forced to crawl.
/// </summary>

public abstract partial class SupportStandingComponent : Component
{
    /// <summary>
    ///     The minimum number of legs needed in order to provide standing support.
    /// </summary>
    [DataField]
    public int MinimumLegCount = 1;
}

/// <summary>
///     Characters with this component can stand with less legs, as long as they have the minimum number of legs.
/// </summary>
[RegisterComponent]
public sealed partial class AlwaysSupportStandingComponent : SupportStandingComponent
{ }

/// <summary>
///     Allows characters with missing legs to stand up while holding this item.
/// </summary>
[RegisterComponent]
public sealed partial class HeldSupportStandingComponent : SupportStandingComponent
{ }

/// <summary>
///     Allows characters with missing legs to stand up while wearing this item.
/// </summary>
[RegisterComponent]
public sealed partial class WornSupportStandingComponent : SupportStandingComponent
{ }
