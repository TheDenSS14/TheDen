// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 BombasterDS
// SPDX-FileCopyrightText: 2025 BombasterDS2
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Goobstation.TapeRecorder;

namespace Content.Client._Goobstation.TapeRecorder;

/// <summary>
/// Required for client side prediction stuff
/// </summary>
public sealed class TapeRecorderSystem : SharedTapeRecorderSystem
{
    private TimeSpan _lastTickTime = TimeSpan.Zero;

    public override void Update(float frameTime)
    {
        if (!Timing.IsFirstTimePredicted)
            return;

        //We need to know the exact time period that has passed since the last update to ensure the tape position is sync'd with the server
        //Since the client can skip frames when lagging, we cannot use frameTime
        var realTime = (float) (Timing.CurTime - _lastTickTime).TotalSeconds;
        _lastTickTime = Timing.CurTime;

        base.Update(realTime);
    }
}
