// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.CartridgeLoader.Cartridges; // DeltaV
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class LogProbeUiState : BoundUserInterfaceState
{
    /// <summary>
    /// The list of probed network devices
    /// </summary>
    public List<PulledAccessLog> PulledLogs;

    /// <summary>
    /// DeltaV: The NanoChat data if a card was scanned, null otherwise
    /// </summary>
    public NanoChatData? NanoChatData { get; }

    public LogProbeUiState(List<PulledAccessLog> pulledLogs, NanoChatData? nanoChatData = null) // DeltaV - NanoChat support
    {
        PulledLogs = pulledLogs;
        NanoChatData = nanoChatData; // DeltaV
    }
}

[Serializable, NetSerializable, DataRecord]
public sealed class PulledAccessLog
{
    public readonly TimeSpan Time;
    public readonly string Accessor;

    public PulledAccessLog(TimeSpan time, string accessor)
    {
        Time = time;
        Accessor = accessor;
    }
}
