// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;

namespace Content.Client.Lamiae;

/// <summary>
/// This system turns on our always-on overlay. I have no opinion on this design pattern or the existence of this file.
/// It also fetches the deps it needs.
/// </summary>
public sealed class SnakeOverlaySystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;

    public override void Initialize()
    {
        base.Initialize();
        _overlay.AddOverlay(new SnakeOverlay(EntityManager, _resourceCache));
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _overlay.RemoveOverlay<SnakeOverlay>();
    }
}
