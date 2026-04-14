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
        if (component.ShaderPrototype is null)
        {
            _sawmill.Info($"Shader prototype was null on component startup.");
            return;
        }

        if (!_prototypeManager.HasIndex<ShaderPrototype>(component.ShaderPrototype))
        {
            _sawmill.Info($"Did not find specified shader prototype: {component.ShaderPrototype} on component startup.");
            return;
        }

        _shader = _prototypeManager.Index<ShaderPrototype>(component.ShaderPrototype).InstanceUnique();

        _shader.SetParameter("noise_texture", _noiseTexture); // we don't need to set this every frame since it's completely static and never changes.

        SetShader(uid, component.Enabled, component);
        Dirty(uid, component);
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

        sprite.PostShader = enabled ? _shader : null;
        //sprite.GetScreenTexture = enabled; // do not pass the screen texture since we're trying to affect the entity's sprite. reminder to myself to not do this
        sprite.RaiseShaderEvent = enabled;
    }
    private void OnShaderRender(EntityUid uid, ApplyShaderToEntityComponent component, BeforePostShaderRenderEvent args)
    {
        foreach (var parameter in component.ShaderParameters)
        {
            _shader.SetParameter(parameter.Key, parameter.Value);
        }
    }
}
