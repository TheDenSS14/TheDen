// SPDX-FileCopyrightText: 2025 Eightballll
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.ParcelWrap.Systems;

[Serializable, NetSerializable]
public sealed partial class ParcelWrapItemDoAfterEvent : SimpleDoAfterEvent;

[Serializable, NetSerializable]
public sealed partial class UnwrapWrappedParcelDoAfterEvent : SimpleDoAfterEvent;
