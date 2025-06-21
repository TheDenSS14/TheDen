// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.Abilities.Borgs;

/// <summary>
/// Marks this entity as being candy with a random flavor and color.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RandomizedCandyComponent : Component
{
}

[Serializable, NetSerializable]
public enum RandomizedCandyVisuals : byte
{
    Color
}
