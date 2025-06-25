// SPDX-FileCopyrightText: 2020 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 FoLoKe <36813380+FoLoKe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Gibbing.Events;
using Content.Shared.Storage;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition.Components
{
    /// <summary>
    /// Indicates that the entity can be thrown on a kitchen spike for butchering.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class ButcherableComponent : Component
    {
        [DataField("spawned", required: true)]
        public List<EntitySpawnEntry> SpawnedEntities = new();

        [ViewVariables(VVAccess.ReadWrite), DataField("butcherDelay")]
        public float ButcherDelay = 8.0f;

        [ViewVariables(VVAccess.ReadWrite), DataField("butcheringType")]
        public ButcheringType Type = ButcheringType.Knife;

        /// <summary>
        /// Prevents butchering same entity on two and more spikes simultaneously and multiple doAfters on the same Spike
        /// </summary>
        [ViewVariables]
        public bool BeingButchered;

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
        Knife, // e.g. goliaths
        Spike, // e.g. monkeys
        Gibber // e.g. humans. TODO
    }
}
