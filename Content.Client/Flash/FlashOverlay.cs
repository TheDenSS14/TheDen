// SPDX-FileCopyrightText: 2020 R. Neuser <rneuser@iastate.edu>
// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 GraniteSidewalk <32942106+GraniteSidewalk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 deathride58 <deathride58@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Viewport;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Client.Player;
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

        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _shader;
        private double _startTime = -1;
        private double _lastsFor = 1;
        private Texture? _screenshotTexture;

        public FlashOverlay()
        {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("FlashedEffect").Instance().Duplicate();
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
