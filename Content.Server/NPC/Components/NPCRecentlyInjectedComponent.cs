// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.Components
{
    /// Added when a medibot injects someone
    /// So they don't get injected again for at least a minute.
    [RegisterComponent]
    public sealed partial class NPCRecentlyInjectedComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite), DataField("accumulator")]
        public float Accumulator = 0f;

        [ViewVariables(VVAccess.ReadWrite), DataField("removeTime")]
        public TimeSpan RemoveTime = TimeSpan.FromMinutes(1);
    }
}
