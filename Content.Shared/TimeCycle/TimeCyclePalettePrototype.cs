// SPDX-FileCopyrightText: 2024 DocNITE <docnite0530@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.TimeCycle;

/// <summary>
///
/// </summary>
[Prototype("timeCyclePalette")]
public sealed partial class TimeCyclePalettePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public Dictionary<int, Color> TimeEntries = default!;
}
