// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.NameModifier.Components;

/// <summary>
/// Used to manage modifiers on an entity's name and handle renaming in a way
/// that survives being renamed by multiple systems.
/// </summary>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(NameModifierSystem))]
public sealed partial class NameModifierComponent : Component
{
    /// <summary>
    /// The entity's name without any modifiers applied.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string BaseName = string.Empty;
}
