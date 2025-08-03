// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Lavaland.Shuttles.Components;
using Content.Shared.Shuttles.Systems;
using Content.Shared.Timing;
using Robust.Shared.Serialization;

namespace Content.Shared._Lavaland.Shuttles;

[Serializable, NetSerializable]
public enum DockingConsoleUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class DockingConsoleState(FTLState ftlState, StartEndTime ftlTime, List<DockingDestination> destinations) : BoundUserInterfaceState
{
    public FTLState FTLState = ftlState;
    public StartEndTime FTLTime = ftlTime;
    public List<DockingDestination> Destinations = destinations;
}

[Serializable, NetSerializable]
public sealed class DockingConsoleFTLMessage(int index) : BoundUserInterfaceMessage
{
    public int Index = index;
}

[Serializable, NetSerializable]
public sealed class DockingConsoleShuttleCheckMessage : BoundUserInterfaceMessage;
