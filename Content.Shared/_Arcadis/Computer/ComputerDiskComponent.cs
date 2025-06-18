// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Arcadis.Computer;

/// <summary>
/// Main component for the ComputerDisk system
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
//[Access(typeof(ComputerDiskSystem))]
public sealed partial class ComputerDiskComponent : Component
{
    /// <summary>
    /// The prototype of the computer that will be used
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId ProgramPrototype;

    [AutoNetworkedField]

    public EntityUid? ProgramPrototypeEntity;

    [DataField, AutoNetworkedField]
    public bool PersistState;
}
