// SPDX-FileCopyrightText: 2021 Kara Dinyes <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Errant <35878406+Errant-4@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Item;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Whitelist;

/// <summary>
///     Used to determine whether an entity fits a certain whitelist.
///     Does not whitelist by prototypes, since that is undesirable; you're better off just adding a tag to all
///     entity prototypes that need to be whitelisted, and checking for that.
/// </summary>
/// <code>
/// whitelist:
///   tags:
///   - Cigarette
///   - FirelockElectronics
///   components:
///   - Buckle
///   - AsteroidRock
///   sizes:
///   - Tiny
///   - Large
/// </code>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class EntityWhitelist
{
    /// <summary>
    ///     Component names that are allowed in the whitelist.
    /// </summary>
    [DataField] public string[]? Components;
    // TODO yaml validation

    /// <summary>
    ///     Mind Role Prototype names that are allowed in the whitelist.
    /// </summary>
    [DataField] public string[]? MindRoles;
    // TODO yaml validation

    /// <summary>
    ///     Item sizes that are allowed in the whitelist.
    /// </summary>
    [DataField]
    public List<ProtoId<ItemSizePrototype>>? Sizes;

    [NonSerialized, Access(typeof(EntityWhitelistSystem))]
    public List<ComponentRegistration>? Registrations;

    /// <summary>
    ///     Tags that are allowed in the whitelist.
    /// </summary>
    [DataField]
    public List<ProtoId<TagPrototype>>? Tags;

    /// <summary>
    ///     If false, an entity only requires one of these components or tags to pass the whitelist. If true, an
    ///     entity requires to have ALL of these components and tags to pass.
    ///     The "Sizes" criteria will ignores this, since an item can only have one size.
    /// </summary>
    [DataField]
    public bool RequireAll;
}
