// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared.Fluids;

public abstract partial class SharedPuddleSystem
{
    // DEN COMMENT: this should be a datafield on reagentprotoype. no idea why its done this way

    private static readonly ProtoId<ReagentPrototype> Water = "Water";
    private static readonly ProtoId<ReagentPrototype> Cum = "Cum"; // DEN: whatever
    private static readonly ProtoId<ReagentPrototype> NaturalLubricant = "NaturalLubricant"; // DEN: whatever

    // DEN COMMENT: if youre gonna do it this way this should probably be a list of protoids instead.
    public static readonly string[] EvaporationReagents = [Water, Cum, NaturalLubricant]; // DEN: add sex fluids to this list

    public bool CanFullyEvaporate(Solution solution)
    {
        return solution.GetTotalPrototypeQuantity(EvaporationReagents) == solution.Volume;
    }
}
