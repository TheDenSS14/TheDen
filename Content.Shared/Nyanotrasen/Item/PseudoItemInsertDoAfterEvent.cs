// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared.Item.PseudoItem;


[Serializable, NetSerializable]
public sealed partial class PseudoItemInsertDoAfterEvent : SimpleDoAfterEvent
{
}

