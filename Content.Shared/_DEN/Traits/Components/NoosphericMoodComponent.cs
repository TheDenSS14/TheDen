// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Traits.EntitySystems;
using Content.Shared.Mood;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Traits.Components;

[RegisterComponent]
[Access(typeof(SharedNoosphericMoodSystem))]
public sealed partial class NoosphericMoodComponent : Component
{
    [DataField("thresholds")]
    public Dictionary<ProtoId<MoodEffectPrototype>, double> GlimmerThresholds = new();
}
