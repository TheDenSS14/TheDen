// SPDX-FileCopyrightText: 2024 Pierson Arnold <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Consent.UI.Windows;
using Content.Client.Gameplay;
using Content.Client.UserInterface.Controls;
using Content.Client.UserInterface.Systems.MenuBar.Widgets;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using JetBrains.Annotations;
using static Robust.Client.UserInterface.Controls.BaseButton;

namespace Content.Client.Consent.UI;

[UsedImplicitly]
public sealed class ConsentUiController : UIController, IOnStateChanged<GameplayState>
{
    [Dependency] private readonly IInputManager _input = default!;

    private ConsentWindow? _window;

    private MenuButton? ConsentButton => UIManager.GetActiveUIWidgetOrNull<GameTopMenuBar>()?.ConsentButton;

    public void OnStateEntered(GameplayState state)
    {
        EnsureWindow();

        _input.SetInputCommand(ContentKeyFunctions.OpenConsentWindow,
            InputCmdHandler.FromDelegate(_ => ToggleWindow()));
    }

    public void OnStateExited(GameplayState state)
    {
        if (_window != null)
        {
            _window.Dispose();
            _window = null;
        }
    }

    public void UnloadButton()
    {
        if (ConsentButton == null)
        {
            return;
        }

        ConsentButton.OnPressed -= ConsentButtonPressed;
    }

    public void LoadButton()
    {
        if (ConsentButton == null)
        {
            return;
        }

        ConsentButton.OnPressed += ConsentButtonPressed;
    }

    private void ConsentButtonPressed(ButtonEventArgs args)
    {
        ToggleWindow();
    }

    private void EnsureWindow()
    {
        if (_window is { Disposed: false })
            return;

        _window = UIManager.CreateWindow<ConsentWindow>();
        _window.OnOpen += () => {
            if (ConsentButton is not null)
                ConsentButton.Pressed = true;
        };
        _window.OnClose += () => {
            if (ConsentButton is not null)
                ConsentButton.Pressed = false;
            _window.UpdateUi(); // Discard unsaved changes
        };
    }

    private void ToggleWindow()
    {
        if (_window is null)
            return;

        UIManager.ClickSound();
        if (_window.IsOpen != true)
        {
            _window.OpenCentered();
        }
        else
        {
            _window.Close();
        }
    }
}
