using Content.Shared._DV.CCVars;
using Content.Shared.CCVar;
using Content.Shared.Drugs;
using Content.Shared.StatusEffect;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.Drugs;

public sealed class MnemolithOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntitySystemManager _sysMan = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;
    public override bool RequestScreenTexture => true;
    private readonly ShaderInstance _mnemolithShader;

    // Ramp timer
    public float Elapsed = 0.0f;
    public float Phase = 0.0f;

    private float _timeScale = 1.0f;
    private float _warpScale = 1.0f;

    // Ramp duration in seconds
    private const float RampDuration = 1.0f; // TODO: Adjust ramp time

    // Effect strength goes from 0 → 1 over RampDuration
    private float EffectScale => Math.Clamp(Elapsed / RampDuration, 0.0f, 1.0f);

    public MnemolithOverlay()
    {
        IoCManager.InjectDependencies(this);

        _mnemolithShader = _prototypeManager.Index<ShaderPrototype>("Mnemolith").InstanceUnique();
        _config.OnValueChanged(DCCVars.DisableDrugWarping, OnDisableDrugWarpingChanged, invokeImmediately: true);
    }

    private void OnDisableDrugWarpingChanged(bool disabled)
    {
        _timeScale = disabled ? 0.0f : 1.0f;
        _warpScale = disabled ? 0.0f : 1.0f;
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        var playerEntity = _playerManager.LocalEntity;
        if (playerEntity == null)
            return;

        // FIX: correct component name
        if (!_entityManager.HasComponent<SeeingMnemolithComponent>(playerEntity)
            || !_entityManager.TryGetComponent<StatusEffectsComponent>(playerEntity, out var status))
            return;

        var statusSys = _sysMan.GetEntitySystem<StatusEffectsSystem>();
        if (!statusSys.TryGetTime(playerEntity.Value, DrugOverlaySystem.MnemolithKey, out var time, status))
            return;

        // Ramp elapsed time up linearly
        Elapsed = Math.Min(Elapsed + args.DeltaSeconds, RampDuration);
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return false;

        if (args.Viewport.Eye != eyeComp.Eye)
            return false;

        // Draw immediately once effect starts
        return Elapsed > 0f;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;

        var handle = args.WorldHandle;
        _mnemolithShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _mnemolithShader.SetParameter("colorScale", EffectScale);
        _mnemolithShader.SetParameter("timeScale", _timeScale);
        _mnemolithShader.SetParameter("warpScale", _warpScale * EffectScale);
        _mnemolithShader.SetParameter("phase", Phase);
        handle.UseShader(_mnemolithShader);
        handle.DrawRect(args.WorldBounds, Color.White);
        handle.UseShader(null);
    }
}
