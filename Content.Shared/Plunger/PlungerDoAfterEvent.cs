// SPDX-FileCopyrightText: 2024 brainfood1183 <113240905+brainfood1183@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT


using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Plunger;

[Serializable, NetSerializable]
public sealed partial class PlungerDoAfterEvent : SimpleDoAfterEvent
{
}
