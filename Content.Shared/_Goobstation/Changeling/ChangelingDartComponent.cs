// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Changeling;

[RegisterComponent]
public sealed partial class ChangelingDartComponent : Component
{
    [DataField(required: true)]
    public ProtoId<ReagentStingConfigurationPrototype> StingConfiguration;

    [DataField]
    public float ReagentDivisor = 2;
}
