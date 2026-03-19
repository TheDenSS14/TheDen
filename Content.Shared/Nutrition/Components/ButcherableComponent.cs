// SPDX-FileCopyrightText: 2020 20kdc
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 FoLoKe
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 ShadowCommander
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2021 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Gibbing.Events;
using Content.Shared.Kitchen;
using Content.Shared.Storage;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition.Components;

/// <summary>
/// Indicates that the entity can be butchered.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ButcherableComponent : Component
{
    /// <summary>
    /// List of the entities that this entity should spawn after being butchered.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="SharedKitchenSpikeSystem"/> spawns one item at a time and decreases the amount until it's zero and then removes the entry.
    /// </remarks>
    [DataField("spawned", required: true), AutoNetworkedField]
    public List<EntitySpawnEntry> SpawnedEntities = [];

    /// <summary>
    /// Time required to butcher that entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ButcherDelay = 8.0f;

    /// <summary>
    /// Tool type used to butcher that entity.
    /// </summary>
    [DataField("butcheringType"), AutoNetworkedField]
    public ButcheringType Type = ButcheringType.Knife;

        /// <summary>
        /// Whether or not butchery products inherit freshness/rotting level of the thing being butchered.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool SpawnedInheritFreshness = true;

        /// <summary>
        /// For products that inherit the butcherable entity's freshness, this is how much extra time
        /// you get before the item spoils, as a flat number.
        /// For example: If a corpse spoils in 10 minutes, and meat spoils in 5 minutes, and the corpse
        /// has 6 minutes on its rotting timer, the meat will have 60% freshness * 5 minutes = 3 minutes,
        /// plus one minute of flat "freshness increase" that brings its Perishable time elapsed down to 2 minutes.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan FreshnessIncrease = TimeSpan.FromMinutes(1.0);

        // Floof section begin
        /// <summary>
        ///     Whether the entities body should be gibbed by butchering.
        /// </summary>
        [DataField]
        public bool GibBody = true, GibOrgans = false;

        /// <summary>
        ///     How to handle the contents of the gib.
        /// </summary>
        [DataField]
        public GibContentsOption GibContents = GibContentsOption.Drop;
        // Floof section end
}

    [Serializable, NetSerializable]
public enum ButcheringType : byte
{
    /// <summary>
    /// E.g. goliaths.
    /// </summary>
    Knife,

    /// <summary>
    /// E.g. monkeys.
    /// </summary>
    Spike,

    /// <summary>
    /// E.g. humans.
    /// </summary>
    Gibber // TODO
}
