// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Humanoid.Markings;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Markings;


/// <summary>
/// This is used for color animations on markings.
/// </summary>
[RegisterComponent]
public sealed partial class AnimatedMarkingsComponent : Component
{
    [DataField]
    public EntProtoId ActionPrototype = "ActionToggleGlowAnimation";

    [ViewVariables]
    public EntityUid? ActionEntity;

    [ViewVariables]
    public bool IsAnimating;

    [ViewVariables]
    public bool StopAnimatingNextFrame;

    [ViewVariables(VVAccess.ReadOnly)]
    public List<ProtoId<MarkingPrototype>> MarkingIds { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, int> MarkingIdToIndex { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, MarkingCategories> MarkingCategories { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, List<int>> IgnoreColorIndices { get; set; } = new();

    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<ProtoId<MarkingPrototype>, Color> InitialColors { get; set; } = new();

    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<ProtoId<MarkingPrototype>, Color> GoalColors { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, float> CurrentStates { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, bool> IsReversing { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<ProtoId<MarkingPrototype>, bool> IsReady { get; set; } = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public float LastState { get; set; } = 0f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float Step { get; set; } = 0.5f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float InterpolationLambda { get; set; } = 0.5f;
}
