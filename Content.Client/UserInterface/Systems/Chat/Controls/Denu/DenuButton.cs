// SPDX-FileCopyrightText: 2025 Cam
// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Resources;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class DenuButton : ContainerButton
{
    public static readonly Color ColorNormal = Color.FromHex("#7b7e9e");
    public static readonly Color ColorHovered = Color.FromHex("#9699bb");
    public static readonly Color ColorPressed = Color.FromHex("#789B8C");

    public readonly DenuUIController DenuUIController;

    private TextureRect? _textureRect;

    public DenuButton()
    {
        DenuUIController = UserInterfaceManager.GetUIController<DenuUIController>();
        InitializeUI();
    }

    public void InitializeUI()
    {
        var filterTexture = IoCManager.Resolve<IResourceCache>()
            .GetTexture("/Textures/_DEN/Interface/Denu.png");

        _textureRect = new TextureRect();
        _textureRect.Texture = filterTexture;
        _textureRect.HorizontalAlignment = HAlignment.Center;
        _textureRect.VerticalAlignment = VAlignment.Center;
        AddChild(_textureRect);

        ToggleMode = true;
        OnToggled += ev => ToggleDenu(ev.Pressed);
    }

    private void ToggleDenu(bool pressed)
    {
        DenuUIController.IsOpen = pressed;
        if (pressed)
            DenuUIController.OpenWindow();
        else
            DenuUIController.CloseWindow();
    }

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
