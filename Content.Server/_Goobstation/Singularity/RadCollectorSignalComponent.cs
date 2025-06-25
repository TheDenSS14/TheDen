// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Server.Singularity;

/// <summary>
/// Emits signals depending on tank pressure for automated radiation collectors.
/// </summary>
[RegisterComponent, Access(typeof(RadCollectorSignalSystem))]
public sealed partial class RadCollectorSignalComponent : Component
{
    [DataField]
    public RadCollectorState LastState = RadCollectorState.Empty;
}

[Serializable]
public enum RadCollectorState : byte
{
    Empty,
    Low,
    Full
}
