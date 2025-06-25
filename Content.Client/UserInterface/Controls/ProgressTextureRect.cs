// SPDX-FileCopyrightText: 2020 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Client.UserInterface.Systems;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls
{
    public sealed class ProgressTextureRect : TextureRect
    {
        public float Progress;

        private readonly ProgressColorSystem _progressColor = IoCManager.Resolve<IEntityManager>().System<ProgressColorSystem>();

        protected override void Draw(DrawingHandleScreen handle)
        {
            var dims = Texture != null ? GetDrawDimensions(Texture) : UIBox2.FromDimensions(Vector2.Zero, PixelSize);
            dims.Top = Math.Max(dims.Bottom - dims.Bottom * Progress,0);
            handle.DrawRect(dims, _progressColor.GetProgressColor(Progress));

            base.Draw(handle);
        }
    }
}
