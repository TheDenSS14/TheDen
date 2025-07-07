// SPDX-FileCopyrightText: 2023 Debug <sidneymaatman@gmail.com>
// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Server.Roboisseur.Roboisseur
{
    [RegisterComponent]
    public sealed partial class RoboisseurComponent : Component
    {
        [ViewVariables]
        [DataField("accumulator")]
        public float Accumulator = 0f;

        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("impatient")]
        public Boolean Impatient { get; set; } = false;

        [ViewVariables]
        [DataField("resetTime")]
        public TimeSpan ResetTime = TimeSpan.FromMinutes(10);

        [DataField("barkAccumulator")]
        public float BarkAccumulator = 0f;

        [DataField("barkTime")]
        public TimeSpan BarkTime = TimeSpan.FromMinutes(1);

        /// <summary>
        ///     Antispam.
        /// </summary>
        public TimeSpan StateTime = default!;

        [DataField("stateCD")]
        public TimeSpan StateCD = TimeSpan.FromSeconds(5);

        [ViewVariables(VVAccess.ReadWrite)]
        public EntityPrototype DesiredPrototype = default!;

        [DataField("demandMessages")]
        public IReadOnlyList<string> DemandMessages = new[]
        {
            "roboisseur-request-1",
            "roboisseur-request-2",
            "roboisseur-request-3",
            "roboisseur-request-4",
            "roboisseur-request-5",
            "roboisseur-request-6"
        };

        [DataField("impatientMessages")]
        public IReadOnlyList<string> ImpatientMessages = new[]
        {
            "roboisseur-request-impatient-1",
            "roboisseur-request-impatient-2",
            "roboisseur-request-impatient-3",
        };

        [DataField("demandMessagesTier2")]
        public IReadOnlyList<string> DemandMessagesTier2 = new[]
        {
            "roboisseur-request-second-1",
            "roboisseur-request-second-2",
            "roboisseur-request-second-3"
        };

        [DataField("rewardMessages")]
        public IReadOnlyList<String> RewardMessages = new[]
        {
            "roboisseur-thanks-1",
            "roboisseur-thanks-2",
            "roboisseur-thanks-3",
            "roboisseur-thanks-4",
            "roboisseur-thanks-5"
        };

        [DataField("rewardMessagesTier2")]
        public IReadOnlyList<String> RewardMessagesTier2 = new[]
        {
            "roboisseur-thanks-second-1",
            "roboisseur-thanks-second-2",
            "roboisseur-thanks-second-3",
            "roboisseur-thanks-second-4",
            "roboisseur-thanks-second-5"
        };

        [DataField("rejectMessages")]
        public IReadOnlyList<String> RejectMessages = new[]
        {
            "roboisseur-deny-1",
            "roboisseur-deny-2",
            "roboisseur-deny-3"
        };

        [DataField("tier2Protos")]
        public List<String> Tier2Protos = new();

        [DataField("tier3Protos")]
        public List<String> Tier3Protos = new();

        [DataField("robossuierRewards")]
        public IReadOnlyList<String> RobossuierRewards = new List<string>();

        [DataField("blacklistedProtos")]
        public IReadOnlyList<String> BlacklistedProtos = new List<string>();
    }
}
