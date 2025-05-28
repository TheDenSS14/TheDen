using Content.Server.GameTicking.Presets;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.PresetPicker;


/// <summary>
/// This is a prototype for picking a prototype for use in presets.
/// </summary>
[Prototype("randomPresetPicker"), PublicAPI]
public sealed partial class RandomPresetPickerPrototype : IPrototype
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
