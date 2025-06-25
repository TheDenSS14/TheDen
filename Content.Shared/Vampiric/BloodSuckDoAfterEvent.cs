// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared.Vampiric
{
    [Serializable, NetSerializable]
    public sealed partial class BloodSuckDoAfterEvent : SimpleDoAfterEvent
    {
    }
}
