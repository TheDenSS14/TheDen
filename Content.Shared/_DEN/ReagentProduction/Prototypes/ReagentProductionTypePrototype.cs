// SPDX-FileCopyrightText: 2026 Alex C
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.ReagentProduction.Prototypes;

[Prototype]
public sealed class ReagentProductionTypePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Reagent to produce
    /// </summary>
    [DataField]
    public ProtoId<ReagentPrototype> Reagent = "Cum";
    /// <summary>
    /// The solution to produce into
    /// </summary>
    [DataField]
    public string SolutionName = "balls"; //cum is stored in the balls?
    /// <summary>
    /// Maximum capacity of the solution
    /// </summary>
    [DataField]
    public FixedPoint2 MaximumCapacity = 30;

    /// <summary>
    /// Maximum amount you can expel at once
    /// </summary>
    [DataField]
    public FixedPoint2 MaximumLoad = 10;

    /// <summary>
    /// Doafter length
    /// </summary>
    [DataField]
    public TimeSpan FillTime = TimeSpan.FromSeconds(3);

    /// <summary>
    /// How many units are produced each update
    /// </summary>
    [DataField]
    public FixedPoint2 UnitsPerProduction = 5;

    /// <summary>
    /// Popup that occurs when your solution is empty
    /// </summary>
    [DataField]
    public string DryPopup = "cum-verb-dry";

    [DataField]
    public string SuccessPopup = "cum-verb-success";
}
