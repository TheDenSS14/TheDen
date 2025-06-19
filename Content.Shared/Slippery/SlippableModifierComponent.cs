// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
namespace Content.Shared.Slippery;

/// <summary>
///   Modifies the duration of slip paralysis on an entity.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlippableModifierComponent : Component
{
    /// <summary>
    ///   What to multiply the paralyze time by.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ParalyzeTimeMultiplier = 1f;
}
