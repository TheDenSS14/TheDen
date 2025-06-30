// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wires.Components;

/// This is used for items slots that require entity to have wire panel for interactions
[RegisterComponent]
[NetworkedComponent]
public sealed partial class ItemSlotsRequirePanelComponent : Component
{
    /// For each slot: true - slot require opened panel for interaction, false - slot require closed panel for interaction
    [DataField]
    public Dictionary<string, bool> Slots = new();
}
