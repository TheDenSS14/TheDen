// SPDX-FileCopyrightText: 2020 R. Neuser
// SPDX-FileCopyrightText: 2021 20kdc
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 GraniteSidewalk
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 deathride58
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Viewport;
using Content.Shared._DEN.CCVars;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client.Flash
{
    public sealed class FlashOverlay : Overlay
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IClyde _displayManager = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IStateManager _stateManager = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IConfigurationManager _cfg = default!; // DEN: Black flash effect

        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _shader;
        private double _startTime = -1;
        private double _lastsFor = 1;
        private Color _fadeColor = Color.White; // DEN: Black Flash
        private Texture? _screenshotTexture;

        public FlashOverlay()
        {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("FlashedEffect").Instance().Duplicate();
            _cfg.OnValueChanged(DenCCVars.BlackFlashEffect, b => _fadeColor = b ? Color.Black : Color.White); // DEN: Black Flash
            _fadeColor = _cfg.GetCVar(DenCCVars.BlackFlashEffect) ? Color.Black : Color.White; // DEN: Black Flash
        }

        public void ReceiveFlash(double duration)
        {
            if (_stateManager.CurrentState is IMainViewportState state)
            {
                state.Viewport.Viewport.Screenshot(image =>
                {
                    var rgba32Image = image.CloneAs<Rgba32>(SixLabors.ImageSharp.Configuration.Default);
                    _screenshotTexture = _displayManager.LoadTextureFromImage(rgba32Image);
                });
            }

            _startTime = _gameTiming.CurTime.TotalSeconds;
            _lastsFor = duration;
        }

        protected override void Draw(in OverlayDrawArgs args)
        {
            if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
                return;

            if (args.Viewport.Eye != eyeComp.Eye)
                return;

            var percentComplete = (float) ((_gameTiming.CurTime.TotalSeconds - _startTime) / _lastsFor);
            if (percentComplete >= 1.0f)
                return;

            var worldHandle = args.WorldHandle;
            worldHandle.UseShader(_shader);
            _shader.SetParameter("percentComplete", percentComplete);
            _shader.SetParameter("fadeColor", _fadeColor); // DEN: Black Flash.

            if (_screenshotTexture != null)
            {
                worldHandle.DrawTextureRectRegion(_screenshotTexture, args.WorldBounds);
            }

            worldHandle.UseShader(null);
        }

        protected override void DisposeBehavior()
        {
            base.DisposeBehavior();
            _screenshotTexture = null;
        }
    }
}
