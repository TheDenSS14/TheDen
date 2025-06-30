// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Sticky.Components;
using DrawDepth;

[RegisterComponent]
public sealed partial class StickyVisualizerComponent : Component
{
    /// <summary>
    ///     What sprite draw depth set when entity stuck.
    /// </summary>
    [DataField("stuckDrawDepth")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int StuckDrawDepth = (int) DrawDepth.Overdoors;

    /// <summary>
    ///     What sprite draw depth set when entity unstuck.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int DefaultDrawDepth;
}

[Serializable, NetSerializable]
public enum StickyVisuals : byte
{
    IsStuck
}
