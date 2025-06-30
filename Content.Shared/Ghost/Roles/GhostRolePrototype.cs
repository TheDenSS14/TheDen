// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Ghost.Roles;

/// <summary>
///     For selectable ghostrole prototypes in ghostrole spawners.
/// </summary>
[Prototype]
public sealed partial class GhostRolePrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     The name of the ghostrole.
    /// </summary>
    [DataField]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     The description of the ghostrole.
    /// </summary>
    [DataField]
    public string Description { get; set; } = default!;

    /// <summary>
    ///     The entity prototype of the ghostrole
    /// </summary>
    [DataField]
    public string EntityPrototype = default!;

    /// <summary>
    ///     Rules of the ghostrole
    /// </summary>
    [DataField]
    public string Rules = default!;
}