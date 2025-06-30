// SPDX-FileCopyrightText: 2025 M3739 <47579354+M3739@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._DV.Abilities.Kitsune;

/// <summary>
/// This component is needed on fox fires so that the owner can properly update upon its destruction.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class FoxfireComponent : Component
{
    /// <summary>
    /// The kitsune that created this fox fire.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Kitsune;
}
