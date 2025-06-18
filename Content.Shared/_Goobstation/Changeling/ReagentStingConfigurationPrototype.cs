// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared.Changeling;

[DataDefinition]
[Prototype("reagentStingConfiguration")]
public sealed partial class ReagentStingConfigurationPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; }

    [DataField(required: true)]
    public Dictionary<string, FixedPoint2> Reagents = new();
}
