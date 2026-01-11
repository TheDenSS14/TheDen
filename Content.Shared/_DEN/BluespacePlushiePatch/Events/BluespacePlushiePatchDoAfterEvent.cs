// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.BluespacePlushiePatch.Events;


[Serializable, NetSerializable]
public sealed partial class BluespacePlushiePatchDoAfterEvent : SimpleDoAfterEvent;
