// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.StepTrigger.Prototypes;

/// <summary>
///     Prototype representing a StepTriggerType in YAML.
///     Meant to only have an ID property, as that is the only thing that
///     gets saved in StepTriggerGroup.
/// </summary>
[Prototype]
public sealed partial class StepTriggerTypePrototype : IPrototype
{
    [ViewVariables, IdDataField]
    public string ID { get; private set; } = default!;
}
