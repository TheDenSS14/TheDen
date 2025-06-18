// SPDX-FileCopyrightText: 2024 Timemaster99 <57200767+Timemaster99@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Silicon.BatteryLocking;

[RegisterComponent]
public sealed partial class BatterySlotRequiresLockComponent : Component
{
    [DataField]
    public string ItemSlot = string.Empty;
}
