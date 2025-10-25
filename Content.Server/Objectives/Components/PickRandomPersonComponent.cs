// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2025 Memeji Dankiri
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Objectives.Systems;
using Content.Shared.Objectives.Components;

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
