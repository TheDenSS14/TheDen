// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 fishfish458 <fishfish458>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Content.Shared.Cloning.CloningConsole;
using Robust.Client.UserInterface;

namespace Content.Client.CloningConsole.UI
{
    [UsedImplicitly]
    public sealed class CloningConsoleBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private CloningConsoleWindow? _window;

        public CloningConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _window = this.CreateWindow<CloningConsoleWindow>();
            _window.Title = Loc.GetString("cloning-console-window-title");

            _window.CloneButton.OnPressed += _ => SendMessage(new UiButtonPressedMessage(UiButton.Clone));
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            _window?.Populate((CloningConsoleBoundUserInterfaceState) state);
        }
    }
}
