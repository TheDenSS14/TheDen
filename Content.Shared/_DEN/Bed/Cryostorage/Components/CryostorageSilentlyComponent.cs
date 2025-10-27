// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Bed.Cryostorage.Systems;
using Robust.Shared.GameStates;


namespace Content.Shared._DEN.Bed.Cryostorage.Components;


/// <summary>
/// Used to add the ability to silently cryo to cryostorage.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(CryoSilentlySystem))]
public sealed partial class CryostorageSilentlyComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string ContainerId;
}
