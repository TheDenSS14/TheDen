// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Network;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Bed.Cryostorage.Components;

[Serializable, NetSerializable]
public sealed class IgnoreCryoMessage : EntityEventArgs
{
    public bool Ignore { get; }

    public IgnoreCryoMessage(bool ignore)
    {
        Ignore = ignore;
    }
}
