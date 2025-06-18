// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Psionics;
using Robust.Shared.Prototypes;

namespace Content.Server.Abilities.Psionics
{
    [RegisterComponent]
    public sealed partial class InnatePsionicPowersComponent : Component
    {
        /// <summary>
        ///     The list of all powers to be added on Startup
        /// </summary>
        [DataField]
        public List<ProtoId<PsionicPowerPrototype>> PowersToAdd = new();
    }
}
