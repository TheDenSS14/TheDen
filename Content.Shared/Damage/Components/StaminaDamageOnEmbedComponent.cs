// SPDX-FileCopyrightText: 2024 Dakamakat
// SPDX-FileCopyrightText: 2025 Eagle-0
// SPDX-FileCopyrightText: 2025 Princess Cheeseballs
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Damage.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

/// <summary>
/// Applies stamina damage when embeds in an entity.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
[AutoGenerateComponentState]
[Access(typeof(SharedStaminaSystem))]
public sealed partial class StaminaDamageOnEmbedComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public float Damage = 10f;

    // goob edit
    [DataField]
    public float Overtime = 0f;
}
