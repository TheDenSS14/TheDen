// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Abilities.Psionics
{
    [RegisterComponent]
    public sealed partial class TinfoilHatComponent : Component
    {
        public bool IsActive = false;

        [DataField("passthrough")]
        public bool Passthrough = false;

        /// <summary>
        /// Whether this will turn to ash when its psionically fried.
        /// </summary>
        [DataField("destroyOnFry")]
        public bool DestroyOnFry = true;
    }
}
