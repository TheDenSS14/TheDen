// SPDX-FileCopyrightText: 2024 dootythefrooty <137359445+dootythefrooty@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Botany
{
    [RegisterComponent]

    public sealed partial class TeleportingTraitComponent : Component
    {
        /// <summary>
        ///     Teleportation radius of produce.
        /// </summary>
        [DataField]
        public float ProduceTeleportRadius;

        /// <summary>
        ///     How much to divide the potency.
        /// </summary>
        [DataField]
        public float PotencyDivide = 10f;

        /// <summary>
        ///     Potency of fruit.
        /// </summary>
        [DataField]
        public float Potency;

        /// <summary>
        ///     Chance of deletion.
        /// </summary>
        [DataField]
        public float DeletionChance = .5f;
    }
}
