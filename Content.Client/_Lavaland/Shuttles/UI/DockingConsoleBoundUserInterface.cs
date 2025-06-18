// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Lavaland.Shuttles;

namespace Content.Client._Lavaland.Shuttles.UI;

public sealed class DockingConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private DockingConsoleWindow? _window;

    public DockingConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = new DockingConsoleWindow(Owner);
        _window.OnFTL += index => SendMessage(new DockingConsoleFTLMessage(index));
        _window.OnShuttleCall += args => SendMessage(new DockingConsoleShuttleCheckMessage());
        _window.OnClose += Close;
        _window.OpenCentered();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is DockingConsoleState cast)
            _window?.UpdateState(cast);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            _window?.Orphan();
    }
}
