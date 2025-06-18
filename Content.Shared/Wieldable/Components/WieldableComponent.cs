// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2023 stopbreaking <126102320+stopbreaking@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Wieldable.Components;

/// <summary>
///     Used for objects that can be wielded in two or more hands,
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(WieldableSystem)), AutoGenerateComponentState]
public sealed partial class WieldableComponent : Component
{
    [DataField("wieldSound")]
    public SoundSpecifier? WieldSound = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg");

    [DataField("unwieldSound")]
    public SoundSpecifier? UnwieldSound;

    /// <summary>
    ///     Number of free hands required (excluding the item itself) required
    ///     to wield it
    /// </summary>
    [DataField("freeHandsRequired")]
    public int FreeHandsRequired = 1;

    [AutoNetworkedField, DataField("wielded")]
    public bool Wielded = false;

    /// <summary>
    ///     Whether using the item inhand while wielding causes the item to unwield.
    ///     Unwielding can conflict with other inhand actions. 
    /// </summary>
    [DataField]
    public bool UnwieldOnUse = true;

    [DataField("wieldedInhandPrefix")]
    public string? WieldedInhandPrefix = "wielded";

    public string? OldInhandPrefix = null;
}

[Serializable, NetSerializable]
public enum WieldableVisuals : byte
{
    Wielded
}
