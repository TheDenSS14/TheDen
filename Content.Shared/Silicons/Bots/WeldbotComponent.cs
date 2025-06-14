using Robust.Shared.Audio;

namespace Content.Shared.Silicons.Bots;

/// <summary>
/// Used by the server for NPC Weldbot welding.
/// Currently no clientside prediction done, only exists in shared for emag handling.
/// </summary>
[RegisterComponent]
[Access(typeof(SharedWeldbotSystem))]
public sealed partial class WeldbotComponent : Component
{
    /// <summary>
    /// Sound played after welding a patient.
    /// </summary>
    [DataField]
    public SoundSpecifier WeldSound = new SoundPathSpecifier("/Audio/Items/welder2.ogg");

    [DataField]
    public SoundSpecifier EmagSparkSound = new SoundCollectionSpecifier("sparks")
    {
        Params = AudioParams.Default.WithVolume(8f)
    };

    public bool IsEmagged = false;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EmaggedBurnDamage = 10;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SiliconRepairAmount = 30;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float StructureRepairAmount = 5;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DoAfterLength = 3.0f;
}
