// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Cabinet;

/// <summary>
///     Used for entities that can be opened, closed, and can hold one item. E.g., fire extinguisher cabinets.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class ItemCabinetComponent : Component
{
    /// <summary>
    ///     Sound to be played when the cabinet door is opened.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? DoorSound;

    /// <summary>
    ///     The <see cref="ItemSlot"/> that stores the actual item. The entity whitelist, sounds, and other
    ///     behaviours are specified by this <see cref="ItemSlot"/> definition.
    /// </summary>
    [DataField, ViewVariables]
    public ItemSlot CabinetSlot = new();

    /// <summary>
    ///     Whether the cabinet is currently open or not.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Opened;

    /// <summary>
    /// The state for when the cabinet is open
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public string? OpenState;

    /// <summary>
    /// The state for when the cabinet is closed
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public string? ClosedState;
}
