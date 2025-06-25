// SPDX-FileCopyrightText: 2024 Aexxie <codyfox.077@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Magnus Larsen <i.am.larsenml@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
namespace Content.Shared.Flash.Components;

/// <summary>
/// Upon being triggered will flash in an area around it.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class FlashOnTriggerComponent : Component
{
    [DataField] public float Range = 1.0f;
    [DataField] public float Duration = 8.0f;
    [DataField] public float Probability = 1.0f;
}
