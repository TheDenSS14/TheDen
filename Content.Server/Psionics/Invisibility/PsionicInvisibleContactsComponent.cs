// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;
using Robust.Shared.Timing;

namespace Content.Server.Psionics
{
    [RegisterComponent]
    public sealed partial class PsionicInvisibleContactsComponent : Component
    {
        [DataField("whitelist", required: true)]
        public EntityWhitelist Whitelist = default!;

        /// <summary>
        /// This tracks how many valid entities are being contacted,
        /// so when you stop touching one, you don't immediately lose invisibility.
        /// </summary>
        [DataField("stages")]
        public int Stages = 0;
    }
}
