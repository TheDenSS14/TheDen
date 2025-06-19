// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Access;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Turrets;

/// <summary>
/// Attached to entities to provide them with turret target selection data.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(TurretTargetSettingsSystem))]
public sealed partial class TurretTargetSettingsComponent : Component
{
    /// <summary>
    /// Crew with one or more access levels from this list are exempt from being targeted by turrets.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<AccessLevelPrototype>> ExemptAccessLevels = new();
}