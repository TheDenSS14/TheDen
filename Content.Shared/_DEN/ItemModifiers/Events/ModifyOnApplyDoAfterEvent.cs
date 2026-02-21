// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.ItemModifiers.Events;


[Serializable, NetSerializable]
public sealed partial class ModifyOnApplyDoAfterEvent : SimpleDoAfterEvent;
