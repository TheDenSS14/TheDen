// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 BombasterDS
// SPDX-FileCopyrightText: 2025 BombasterDS2
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.TapeRecorder;

/// <summary>
/// Added to tape records that are updating, winding or rewinding the tape.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ActiveTapeRecorderComponent : Component;
