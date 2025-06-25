// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Clothing;

/// <summary>
/// Modifies speed when worn and activated.
/// Supports <c>ItemToggleComponent</c>.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(ClothingSpeedModifierSystem))]
public sealed partial class ClothingSpeedModifierComponent : Component
{
    [DataField]
    public float WalkModifier = 1.0f;

    [DataField]
    public float SprintModifier = 1.0f;

    [DataField]
    public bool RequiresToggle;
}

[Serializable, NetSerializable]
public sealed class ClothingSpeedModifierComponentState : ComponentState
{
    public float WalkModifier;
    public float SprintModifier;

    public ClothingSpeedModifierComponentState(float walkModifier, float sprintModifier)
    {
        WalkModifier = walkModifier;
        SprintModifier = sprintModifier;
    }
}
