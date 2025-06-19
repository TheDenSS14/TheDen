// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nyanotrasen.Research.SophicScribe;

[RegisterComponent]
public sealed partial class SophicScribeComponent : Component
{
    [DataField("accumulator")]
    public float Accumulator;

    [DataField("announceInterval")]
    public TimeSpan AnnounceInterval = TimeSpan.FromMinutes(2);

    [DataField("nextAnnounce")]
    public TimeSpan NextAnnounceTime;

    /// <summary>
    ///     Antispam.
    /// </summary>
    public TimeSpan StateTime = default!;

    [DataField("stateCD")]
    public TimeSpan StateCD = TimeSpan.FromSeconds(5);
}
