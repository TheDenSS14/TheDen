// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.GameTicking.Presets;
using Robust.Shared.Prototypes;

namespace Content.Server.PresetPicker;

/// <summary>
/// This is a prototype for picking a prototype for use in presets.
/// </summary>
[Prototype]
public sealed partial class PresetPickerPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Choose from a list of presets.
    /// </summary>
    [DataField]
    public List<string>? PossiblePresets;

    /// <summary>
    ///     Choose from a list of presets with a weight
    /// </summary>
    [DataField]
    public Dictionary<string, float>? PossibleWeightedPresets;
}
