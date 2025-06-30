// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;

namespace Content.Client.UserInterface;

public sealed class GlimmerGraph : Control
{
    private readonly IResourceCache _resourceCache;
    private readonly List<int> _glimmer;
    private const int XOffset = 15;
    private const int YOffset = 210;
    private const int Length = 450;
    private static int YOffsetTop => YOffset - 200;

    public GlimmerGraph(IResourceCache resourceCache, List<int> glimmer)
    {
        _resourceCache = resourceCache;
        _glimmer = glimmer;
        HorizontalAlignment = HAlignment.Left;
        VerticalAlignment = VAlignment.Bottom;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        base.Draw(handle);
        var box = new UIBox2(new Vector2(XOffset, YOffset), new Vector2(XOffset + Length, YOffsetTop));
        handle.DrawRect(box, Color.FromHex("#424245"));
        var texture = _resourceCache.GetTexture("/Textures/Interface/glimmerGraph.png");
        handle.DrawTexture(texture, new Vector2(XOffset, YOffsetTop));

        if (_glimmer.Count < 2)
            return;

        var spacing = Length / (_glimmer.Count - 1);

        var i = 0;
        while (i + 1 < _glimmer.Count)
        {
            var vector1 = new Vector2(XOffset + i * spacing, YOffset - _glimmer[i] / 5);
            var vector2 = new Vector2(XOffset + (i + 1) * spacing, YOffset - _glimmer[i + 1] / 5);
            handle.DrawLine(vector1, vector2, Color.FromHex("#A200BB"));
            handle.DrawLine(vector1 + new Vector2(0, 1), vector2 + new Vector2(0, 1), Color.FromHex("#A200BB"));
            handle.DrawLine(vector1 - new Vector2(0, 1), vector2 - new Vector2(0, 1), Color.FromHex("#A200BB"));
            handle.DrawLine(new Vector2(XOffset + i * spacing, YOffset), new Vector2(XOffset + i * spacing, YOffsetTop), Color.FromHex("#686868"));
            i++;
        }
    }
}

