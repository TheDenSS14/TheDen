// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared.SprayPainter.Prototypes;

/// <summary>
/// Maps airlock style names to department ids.
/// </summary>
[Prototype("airlockDepartments")]
public sealed partial class AirlockDepartmentsPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Dictionary of style names to department ids.
    /// If a style does not have a department (e.g. external) it is set to null.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, ProtoId<DepartmentPrototype>> Departments = new();
}
