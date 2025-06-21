// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Shipyard.Prototypes;

/// <summary>
/// Like <c>TagPrototype</c> but for vessel categories.
/// Prevents making typos being silently ignored by the linter.
/// </summary>
[Prototype("vesselCategory")]
public sealed class VesselCategoryPrototype : IPrototype
{
    [ViewVariables, IdDataField]
    public string ID { get; } = default!;
}
