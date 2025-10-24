// SPDX-FileCopyrightText: 2024 Rinary
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Content.Client.Resources;
using Content.Shared.StatusIcon;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Client.ResourceManagement;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.RichText;

public sealed class LogoTag : IMarkupTag
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystem = default!;
    private SpriteSystem? _spriteSystem;
    private IResourceCache? _resourceCache;

    public string Name => "logo";

    public bool TryGetControl(MarkupNode node, [NotNullWhen(true)] out Control? control)
    {
        _spriteSystem ??= _entitySystem.GetEntitySystem<SpriteSystem>();
        _resourceCache ??= IoCManager.Resolve<IResourceCache>();
        
        var icon = new TextureRect
        {
            Texture = _resourceCache.GetTexture("/Textures/_Starlight/Logo/Nanotrasen_Logo.png"),
            TextureScale = new Vector2(0.5f, 0.5f),
        };
        
        control = icon;
        return true;
    }
}