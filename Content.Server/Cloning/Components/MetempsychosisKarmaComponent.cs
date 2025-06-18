// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Cloning.Components
{
    /// <summary>
    /// This tracks how many times you have already been cloned and lowers your chance of getting a humanoid each time.
    /// </summary>
    [RegisterComponent]
    public sealed partial class MetempsychosisKarmaComponent : Component
    {
        [DataField]
        public int Score = 0;
    }
}
