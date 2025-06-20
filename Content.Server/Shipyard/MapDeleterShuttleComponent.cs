// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Shipyard;

/// <summary>
/// When added to a shuttle, once it FTLs the previous map is deleted.
/// After that the component is removed to prevent insane abuse.
/// </summary>
/// <remarks>
/// Could be upstreamed at some point, loneop shuttle could use it.
/// </remarks>
[RegisterComponent, Access(typeof(MapDeleterShuttleSystem))]
public sealed partial class MapDeleterShuttleComponent : Component
{
    /// <summary>
    /// Only set by the system to prevent someone in VV deleting maps by mistake or otherwise.
    /// </summary>
    public bool Enabled;
}
