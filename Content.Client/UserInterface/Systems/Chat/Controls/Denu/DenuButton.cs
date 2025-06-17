﻿using Content.Client.Resources;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class DenuButton : ChatPopupButton<DenuPopup>
{
    public static readonly Color ColorNormal = Color.FromHex("#7b7e9e");
    public static readonly Color ColorHovered = Color.FromHex("#9699bb");
    public static readonly Color ColorPressed = Color.FromHex("#789B8C");

    private readonly TextureRect? _textureRect;

    public DenuButton()
    {
        var filterTexture = IoCManager.Resolve<IResourceCache>()
            .GetTexture("/Textures/_DEN/Interface/Denu.png");

        _textureRect = new()
        {
            Texture = filterTexture,
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center
        };

        AddChild(_textureRect);
    }

    protected override UIBox2 GetPopupPosition() =>
        UIBox2.FromDimensions(GlobalPosition, Popup.MinSize with { X = Math.Max(Popup.MinSize.X, Popup.MinWidth) });

    private void UpdateChildColors()
    {
        if (_textureRect == null)
            return;

        _textureRect.ModulateSelfOverride = DrawMode switch
        {
            DrawModeEnum.Normal => ColorNormal,
            DrawModeEnum.Pressed => ColorPressed,
            DrawModeEnum.Hover => ColorHovered,
            DrawModeEnum.Disabled => Color.Transparent,
            _ => ColorNormal
        };
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();
        UpdateChildColors();
    }

    protected override void StylePropertiesChanged()
    {
        base.StylePropertiesChanged();
        UpdateChildColors();
    }
}
