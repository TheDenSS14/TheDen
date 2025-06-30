// SPDX-FileCopyrightText: 2023 Adrian16199 <144424013+Adrian16199@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;

namespace Content.Server.Carrying
{
    [RegisterComponent]
    public sealed partial class CarriableComponent : Component
    {
        /// <summary>
        ///     Number of free hands required
        ///     to carry the entity
        /// </summary>
        [DataField]
        public int FreeHandsRequired = 2;

        public CancellationTokenSource? CancelToken;

        /// <summary>
        ///     The base duration (In Seconds) of how long it should take to pick up this entity
        ///     before Contests are considered.
        /// </summary>
        [DataField]
        public float PickupDuration = 3;
    }
}
