using Content.Server.GameTicking.Presets;
using Robust.Shared.Prototypes;


namespace Content.Server.PresetPicker;


/// <summary>
/// This is a prototype for picking a prototype for use in presets.
/// </summary>
[Prototype("presetPicker")]
public sealed partial class PresetPickerPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    /// <summary>
    ///     Choose from a list of presets.
    /// </summary>
    [DataField]
    public List<ProtoId<GamePresetPrototype>>? PossiblePresets;

    /// <summary>
    ///     Choose from a list of presets with a weight
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<GamePresetPrototype>, float>? PossibleWeightedPresets;
}
