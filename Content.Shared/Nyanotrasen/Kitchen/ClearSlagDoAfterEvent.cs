// SPDX-FileCopyrightText: 2023 JJ <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Nyanotrasen.Kitchen
{
    [Serializable, NetSerializable]
    public sealed partial class ClearSlagDoAfterEvent : DoAfterEvent
    {
        [DataField("solution", required: true)]
        public Solution Solution = default!;

        [DataField("amount", required: true)]
        public FixedPoint2 Amount;

        private ClearSlagDoAfterEvent()
        {
        }

        public ClearSlagDoAfterEvent(Solution solution, FixedPoint2 amount)
        {
            Solution = solution;
            Amount = amount;
        }

        public override DoAfterEvent Clone() => this;
    }
}
