// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;

namespace Content.Client.NPC.HTN;

public sealed class HTNOverlay : Overlay
{
    private readonly IEntityManager _entManager = default!;
    private readonly Font _font = default!;

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    public HTNOverlay(IEntityManager entManager, IResourceCache resourceCache)
    {
        _entManager = entManager;
        _font = new VectorFont(resourceCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf"), 10);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (args.ViewportControl == null)
            return;

        var handle = args.ScreenHandle;

        foreach (var (comp, xform) in _entManager.EntityQuery<HTNComponent, TransformComponent>(true))
        {
            if (string.IsNullOrEmpty(comp.DebugText) || xform.MapID != args.MapId)
                continue;

            var worldPos = xform.WorldPosition;

            if (!args.WorldAABB.Contains(worldPos))
                continue;

            var screenPos = args.ViewportControl.WorldToScreen(worldPos);
            handle.DrawString(_font, screenPos + new Vector2(0, 10f), comp.DebugText, Color.White);
        }
    }
}
