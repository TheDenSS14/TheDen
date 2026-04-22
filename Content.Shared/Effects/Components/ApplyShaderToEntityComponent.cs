using Robust.Shared.GameStates;

namespace Content.Shared.Effects.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class ApplyShaderToEntityComponent : Component
{
    /// <summary>
    /// Whether the shader is currently enabled.
    /// </summary>
    [DataField("enabled")]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool Enabled = true;

    /// <summary>
    /// The id of the shader prototype to be applied to the entity.
    /// </summary>
    [DataField("shaderProto", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public string ShaderPrototypeId;

    /// <summary>
    /// Whether or not to pass the screen texture to the shader. This might have a fairly significant performance impact with a lot of shaded entities on screen.
    /// </summary>
    [DataField("passScreen")]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool PassScreenTexture = false;

    /// <summary>
    /// The shader parameters, a dict constructed like "shaderParameter": value
    /// You will need to look into the actual shader's code for parameter names and what kind of values they take.
    /// Unfortunately, for now, I simply don't know how to parse anything other than floats, so shaders with parameters expressed in vectors cannot be changed here.
    /// </summary>
    [DataField("shaderParams")]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public Dictionary<string, float> ShaderParameters = new Dictionary<string, float> { };

}
