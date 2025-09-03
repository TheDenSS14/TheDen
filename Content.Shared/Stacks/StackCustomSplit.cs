// SPDX-FileCopyrightText: 2024 Whatstone
//
// SPDX-License-Identifier: AGPL-3.0-or-later

// Cherry-pick space-station-14#32938 courtesy of Ilya246
using Robust.Shared.Serialization;

namespace Content.Shared.Stacks
{
    [Serializable, NetSerializable]
    public sealed class StackCustomSplitAmountMessage : BoundUserInterfaceMessage
    {
        public int Amount;

        public StackCustomSplitAmountMessage(int amount)
        {
            Amount = amount;
        }
    }

    [Serializable, NetSerializable]
    public enum StackCustomSplitUiKey
    {
        Key,
    }
}