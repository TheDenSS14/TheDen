// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Forensics
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ScentTrackerComponent : Component
    {
        /// <summary>
        ///     The currently tracked scent.
        /// </summary>
        [DataField, AutoNetworkedField]
        public string Scent = String.Empty;

        /// <summary>
        ///     The time (in seconds) that it takes to sniff an entity.
        /// </summary>
        [DataField]
        public float SniffDelay = 5.0f;
    }
}
