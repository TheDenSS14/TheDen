// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Objectives.Components;

// DEN Change: This was moved from Server to Shared.

//Floofstation Target Consent Traits: Start
[Flags]
[Serializable, NetSerializable] // DEN: Needed to make it editable
public enum ObjectiveTypes : sbyte
{
    // Unspecified = -1, # DEN: Commented because unused. Fun fact: selecting this makes flags think they're all selected
    TraitorNonTargetable = 0,
    TraitorKill = 1 << 0,
    TraitorTeach = 1 << 1
}
//Floofstation Target Consent Traits: End
