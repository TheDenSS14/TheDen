// SPDX-FileCopyrightText: 2025 Whatstone
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.Mech.Equipment.Events;

[Serializable, NetSerializable]
public sealed partial class ForkRemoveDoAfterEvent : SimpleDoAfterEvent;
