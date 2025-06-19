// SPDX-FileCopyrightText: 2024 Krunklehorn <42424291+krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Preferences
{
    /// <summary>
    /// The spawn priority preference for a profile. Stored in database!
    /// </summary>
    public enum SpawnPriorityPreference
    {
        ///////////////////////
        /// DO NOT TOUCH!!! ///
        ///////////////////////
        None = 0,
        Arrivals = 1,
        Cryosleep = 2,
    }
}
