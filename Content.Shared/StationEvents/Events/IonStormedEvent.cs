// SPDX-FileCopyrightText: 2025 Eightballll
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.StationEvents.Events;

[Serializable, NetSerializable]
public sealed partial class IonStormedEvent(NetEntity station, NetEntity gamerule) : EntityEventArgs
{
    public readonly NetEntity Station = station;
    public readonly NetEntity Gamerule = gamerule;
}
