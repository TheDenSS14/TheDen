// SPDX-FileCopyrightText: 2025 Eightballll
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.ParcelWrap.Components;

/// <summary>
/// This enum is used to change the sprite used by WrappedParcels based on the parcel's size.
/// </summary>
[Serializable, NetSerializable]
public enum WrappedParcelVisuals : byte
{
    Size,
    Layer,
}
