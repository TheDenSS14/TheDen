// SPDX-FileCopyrightText: 2026 MajorMoth
//
// SPDX-License-Identifier: MIT

using Content.Shared.Effects.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.Effects.Systems;

public sealed class ApplyShaderToEntitySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
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
        SubscribeLocalEvent<ApplyShaderToEntityComponent, AfterAutoHandleStateEvent>(OnHandleState);

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
    private void OnHandleState(Entity<ApplyShaderToEntityComponent> entity, ref AfterAutoHandleStateEvent evt)
    {
        SetShader(entity, entity.Comp.Enabled, entity.Comp);
    }

    private void SetShader(EntityUid uid, bool enabled, ApplyShaderToEntityComponent? component = null, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref component, ref sprite, false))
            return;

        if (!ValidateShaderId(uid, component.ShaderPrototypeId))
            return;

        _shader = _prototypeManager.Index<ShaderPrototype>(component.ShaderPrototypeId).InstanceUnique();

        sprite.PostShader = enabled ? _shader : null;
        sprite.GetScreenTexture = component.PassScreenTexture && enabled;
        sprite.RaiseShaderEvent = enabled;

        _shader.SetParameter("noise_texture", _noiseTexture); // we don't need to set this every frame since it's completely static and never changes.
    }
    private bool ValidateShaderId(EntityUid uid, string shaderPrototypeId)
    {
        if (shaderPrototypeId is null)
        {
            _sawmill.Info($"Shader prototype on entity {uid} was null.");
            return false;
        }

        if (!_prototypeManager.HasIndex<ShaderPrototype>(shaderPrototypeId))
        {
            _sawmill.Info($"Did not find specified shader prototype: {shaderPrototypeId} on entity {uid}");
            return false;
        }

        return true;
    }
    private void OnShaderRender(EntityUid uid, ApplyShaderToEntityComponent component, BeforePostShaderRenderEvent args)
    {
        foreach (var parameter in component.ShaderParameters)
        {
            _shader.SetParameter(parameter.Key, parameter.Value);
        }
    }
}
