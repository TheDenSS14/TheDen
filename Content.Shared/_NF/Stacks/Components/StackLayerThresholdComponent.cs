// SPDX-FileCopyrightText: 2024 Unkn0wn_Gh0st <shadowstalkermll@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Stacks.Components;

[RegisterComponent]
public sealed partial class StackLayerThresholdComponent : Component
{
    /// <summary>
    /// A list of thresholds to check against the number of things in the stack.
    /// Each exceeded threshold will cause the next layer to be displayed.
    /// Should be sorted in ascending order.
    /// </summary>
    [DataField(required: true)]
    public List<int> Thresholds = new List<int>();
}
