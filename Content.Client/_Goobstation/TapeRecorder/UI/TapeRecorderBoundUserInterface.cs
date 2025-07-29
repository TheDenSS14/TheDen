// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 BombasterDS
// SPDX-FileCopyrightText: 2025 BombasterDS2
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Goobstation.TapeRecorder;
using Robust.Client.UserInterface;
using Robust.Shared.Timing;

namespace Content.Client._Goobstation.TapeRecorder.UI;

public sealed class TapeRecorderBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private TapeRecorderWindow? _window;

    [ViewVariables]
    private TimeSpan _printCooldown;

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<TapeRecorderWindow>();
        _window.Owner = Owner;
        _window.OnModeChanged += mode => SendMessage(new ChangeModeTapeRecorderMessage(mode));
        _window.OnPrintTranscript += PrintTranscript;
    }

    private void PrintTranscript()
    {
        SendMessage(new PrintTapeRecorderMessage());

        _window?.UpdatePrint(true);

        Timer.Spawn(_printCooldown, () =>
        {
            _window?.UpdatePrint(false);
        });
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not TapeRecorderState cast)
            return;

        _printCooldown = cast.PrintCooldown;

        _window?.UpdateState(cast);
    }
}
