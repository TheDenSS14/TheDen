using Content.Shared.Effects.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Effects.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedApplyShaderToEntitySystem)), AutoGenerateComponentState]
public sealed partial class ApplyShaderToEntityComponent : Component
{
    /// <summary>
    /// Whether the shader is currently enabled.
    /// </summary>
    [DataField("enabled")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled = true;

    /// <summary>
    /// The shader prototype to be applied to the entity.
    /// </summary>
    [DataField("shaderProto", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public string ShaderPrototype;

    /// <summary>
    /// The shader parameters, a dict constructed like "shaderParameter": value
    /// You will need to look into the actual shader's code for parameter names and what kind of values they take.
    /// Unfortunately, for now, I simply don't know how to parse anything other than floats, so shaders with parameters expressed in vectors cannot be changed here.
    /// </summary>
    [DataField("shaderParams")]
    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<string, float> ShaderParameters = new Dictionary<string, float> { };

}
