using Robust.Shared.Prototypes;

namespace Content.Shared.Effects.Components;

[RegisterComponent]
public sealed partial class ApplyShaderToEntityComponent : Component
{
    /// <summary>
    /// Whether the shader is currently enabled.
    /// </summary>
    [DataField("enabled")]
    public bool Enabled = true;

    /// <summary>
    /// The shader prototype to be applied to the entity.
    /// </summary>
    [DataField("shaderProto")]
    public string ShaderPrototype = "AnalogDistortion";

    /// <summary>
    /// The shader parameters, a list of tuples constructed like ("parameter name", floatvalue).
    /// </summary>
    [DataField("shaderParams")]
    public List<(string, float)> ShaderParameters = new List<(string, float)>
    {
        ("tape_crease_smear", 2f),
        ("tape_crease_intensity", 2f),
        ("tape_crease_speed", .2f)
    };

}
