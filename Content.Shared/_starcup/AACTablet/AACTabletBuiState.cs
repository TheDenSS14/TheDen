// SPDX-FileCopyrightText: 2025 little-meow-meow
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._starcup.AACTablet;

[Serializable, NetSerializable]
public sealed class AACTabletBuiState(HashSet<string> radioChannels) : BoundUserInterfaceState
{
    public HashSet<string> RadioChannels = radioChannels;
}
