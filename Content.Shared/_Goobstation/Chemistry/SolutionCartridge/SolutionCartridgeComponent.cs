// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Components;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Chemistry.SolutionCartridge;

[RegisterComponent, NetworkedComponent]
public sealed partial class SolutionCartridgeComponent : Component
{
    [DataField]
    public string TargetSolution = "default";

    [DataField(required: true)]
    public Solution Solution;
}
