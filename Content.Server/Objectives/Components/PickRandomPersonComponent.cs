// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// Sets the target for <see cref="TargetObjectiveComponent"/> to a random person.
/// </summary>
[RegisterComponent, Access(typeof(KillPersonConditionSystem))]
public sealed partial class PickRandomPersonComponent : Component
{
    [DataField]
    public bool NeedsOrganic; // Goobstation: Only pick non-silicon players.

    //Floofstation Target Consent Traits: Start
    [DataField]
    public ObjectiveTypes ObjectiveType;
    //Floofstation Target Consent Traits: End
}

//Floofstation Target Consent Traits: Start
[Flags]
public enum ObjectiveTypes
{
    Unspecified = -1,
    TraitorNonTargetable = 0,
    TraitorKill = 1 << 0,
    TraitorTeach = 1 << 1
}
//Floofstation Target Consent Traits: End
