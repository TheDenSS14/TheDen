using Content.Shared._DV.CCVars;
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

    public float Intoxication = 0.0f;
    public float TimeTicker = 0.0f;
    public float Phase = 0.0f;

    private const float VisualThreshold = 10.0f;
    private const float PowerDivisor = 250.0f;
    private float _timeScale = 0.0f;
    private float _warpScale = 0.0f;

    private float EffectScale => Math.Clamp((Intoxication - VisualThreshold) / PowerDivisor, 0.0f, 1.0f);

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

        if (!_entityManager.HasComponent<SeeingMnemolithComponent>(playerEntity)
            || !_entityManager.TryGetComponent<StatusEffectsComponent>(playerEntity, out var status))
            return;

        var statusSys = _sysMan.GetEntitySystem<StatusEffectsSystem>();
        if (!statusSys.TryGetTime(playerEntity.Value, DrugOverlaySystem.MnemolithKey, out var time, status))
            return;

        var timeLeft = (float)(time.Value.Item2 - time.Value.Item1).TotalSeconds;

        TimeTicker += args.DeltaSeconds;

        if (timeLeft - TimeTicker > timeLeft / 16f)
        {
            Intoxication += (timeLeft - Intoxication) * args.DeltaSeconds / 16f;
        }
        else
        {
            Intoxication -= Intoxication / (timeLeft - TimeTicker) * args.DeltaSeconds;
        }
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return false;

        if (args.Viewport.Eye != eyeComp.Eye)
            return false;

        return EffectScale > 0;
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
