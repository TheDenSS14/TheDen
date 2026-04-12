using Content.Shared.Effects.Components;
using Content.Shared.Effects.Systems;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.Effects.Systems;

public sealed class ApplyShaderToEntitySystem : SharedApplyShaderToEntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SpriteSystem _spriteSystem = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    private ISawmill _sawmill = default!;
    private static readonly ResPath NoiseTexturePath = new("/Textures/Parallaxes/noise.png");
    private Texture _noiseTexture = default!;
    private ShaderInstance _shader = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ApplyShaderToEntityComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ApplyShaderToEntityComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ApplyShaderToEntityComponent, BeforePostShaderRenderEvent>(OnShaderRender);

        _sawmill = _logManager.GetSawmill("ApplyShaderToEntity");

        var noiseSprite = new SpriteSpecifier.Texture(NoiseTexturePath);
        _noiseTexture = _spriteSystem.Frame0(noiseSprite);
    }
    private void OnStartup(EntityUid uid, ApplyShaderToEntityComponent component, ComponentStartup args)
    {
        SetShader(uid, component.Enabled, component);
    }

    private void OnShutdown(EntityUid uid, ApplyShaderToEntityComponent component, ComponentShutdown args)
    {
        if (!Terminating(uid))
            SetShader(uid, false, component);
    }

    private void SetShader(EntityUid uid, bool enabled, ApplyShaderToEntityComponent? component = null, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref component, ref sprite, false))
            return;
        if (!_prototypeManager.HasIndex<ShaderPrototype>(component.ShaderPrototype))
        {
            _sawmill.Info($"Did not find specified shader prototype: {component.ShaderPrototype}");
            return;
        }

        _shader = _prototypeManager.Index<ShaderPrototype>(component.ShaderPrototype).InstanceUnique();

        sprite.PostShader = enabled ? _shader : null;
        //sprite.GetScreenTexture = enabled;
        sprite.RaiseShaderEvent = enabled;
    }
    private void OnShaderRender(EntityUid uid, ApplyShaderToEntityComponent component, BeforePostShaderRenderEvent args)
    {
        _shader.SetParameter("noise_texture", _noiseTexture);

        foreach (var parameter in component.ShaderParameters)
        {
            _shader.SetParameter(parameter.Item1, parameter.Item2);
        }
    }
}
