// SPDX-FileCopyrightText: 2023 AJCM-git
// SPDX-FileCopyrightText: 2023 Flipp Syder
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client._Floof.Consent.UI;
using Content.Client.UserInterface.Systems.Actions;
using Content.Client.UserInterface.Systems.Admin;
using Content.Client.UserInterface.Systems.Bwoink;
using Content.Client.UserInterface.Systems.Character;
using Content.Client.UserInterface.Systems.Crafting;
using Content.Client.UserInterface.Systems.Emotes;
using Content.Client.UserInterface.Systems.EscapeMenu;
using Content.Client.UserInterface.Systems.Gameplay;
using Content.Client.UserInterface.Systems.Guidebook;
using Content.Client.UserInterface.Systems.MenuBar.Widgets;
using Content.Client.UserInterface.Systems.Sandbox;
using Robust.Client.UserInterface.Controllers;
using Content.Client.UserInterface.Systems.Language;

namespace Content.Client.UserInterface.Systems.MenuBar;

public sealed class GameTopMenuBarUIController : UIController
{
    [Dependency] private readonly EscapeUIController _escape = default!;
    [Dependency] private readonly AdminUIController _admin = default!;
    [Dependency] private readonly CharacterUIController _character = default!;
    [Dependency] private readonly CraftingUIController _crafting = default!;
    [Dependency] private readonly AHelpUIController _ahelp = default!;
    [Dependency] private readonly ActionUIController _action = default!;
    [Dependency] private readonly SandboxUIController _sandbox = default!;
    [Dependency] private readonly GuidebookUIController _guidebook = default!;
    [Dependency] private readonly EmotesUIController _emotes = default!;
    [Dependency] private readonly LanguageMenuUIController _language = default!;
    [Dependency] private readonly ConsentUiController _consent = default!;

    private GameTopMenuBar? GameTopMenuBar => UIManager.GetActiveUIWidgetOrNull<GameTopMenuBar>();

    public override void Initialize()
    {
        base.Initialize();

        var gameplayStateLoad = UIManager.GetUIController<GameplayStateLoadController>();
        gameplayStateLoad.OnScreenLoad += LoadButtons;
        gameplayStateLoad.OnScreenUnload += UnloadButtons;
    }

    public void UnloadButtons()
    {
        _escape.UnloadButton();
        _guidebook.UnloadButton();
        _admin.UnloadButton();
        _character.UnloadButton();
        _crafting.UnloadButton();
        _ahelp.UnloadButton();
        _action.UnloadButton();
        _sandbox.UnloadButton();
        _emotes.UnloadButton();
        _language.UnloadButton();
        _consent.UnloadButton();
    }

    public void LoadButtons()
    {
        _escape.LoadButton();
        _guidebook.LoadButton();
        _admin.LoadButton();
        _character.LoadButton();
        _crafting.LoadButton();
        _ahelp.LoadButton();
        _action.LoadButton();
        _sandbox.LoadButton();
        _emotes.LoadButton();
        _language.LoadButton();
        _consent.LoadButton();
    }
}
