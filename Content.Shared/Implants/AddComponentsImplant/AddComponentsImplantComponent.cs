// SPDX-FileCopyrightText: 2025 HoboLyra <112722819+hobolyra@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Implants.AddComponentsImplant;

/// <summary>
///     When added to an implanter will add the passed in components to the implanted entity.
/// </summary>
/// <remarks>
///     Warning: Multiple implants with this component adding the same components will not properly remove components
///     unless removed in the inverse order of their injection (Last in, first out).
/// </remarks>
[RegisterComponent]
public sealed partial class AddComponentsImplantComponent : Component
{
    /// <summary>
    ///     What components will be added to the entity. If the component already exists, it will be skipped.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry ComponentsToAdd;

    /// <summary>
    ///     What components were added to the entity after implanted. Is used to know what components to remove.
    /// </summary>
    [DataField]
    public ComponentRegistry AddedComponents = new();
}
