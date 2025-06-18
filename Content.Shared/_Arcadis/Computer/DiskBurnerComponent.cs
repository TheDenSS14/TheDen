// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Arcadis.Computer;

/// <summary>
/// Component responsible for handling DiskBurner behaviour
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DiskBurnerComponent : Component {

    [DataField]
    public string DiskSlot = "diskSlot";

    [DataField]
    public string BoardSlot = "boardSlot";

    [DataField]
    public string VerbName = "Burn Disk";
}
