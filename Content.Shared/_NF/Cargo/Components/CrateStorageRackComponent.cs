// SPDX-FileCopyrightText: 2025 Whatstone
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared._NF.Cargo.Components;

/// <summary>
/// Designates an entity as a rack for storing trading crates.
/// </summary>
[RegisterComponent]
public sealed partial class CrateStorageRackComponent : Component
{
    [DataField]
    public int MaxObjectsStored = 4;

    [DataField]
    public string ContainerName = "item-container";
}
