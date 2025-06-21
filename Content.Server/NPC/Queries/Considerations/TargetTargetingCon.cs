// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.NPC.Queries.Considerations;

/// <summary>
/// Returns 0f if the NPC has a <see cref="TurretTargetSettingsComponent"/> and the
/// target entity is exempt from being targeted, otherwise it returns 1f.
/// See <see cref="TurretTargetSettingsSystem.EntityIsTargetForTurret"/>
/// for further details on turret target validation.
/// </summary>
public sealed partial class TurretTargetingCon : UtilityConsideration
{

}