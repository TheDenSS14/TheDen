// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Holosign.Events;

[Serializable, NetSerializable]
public enum LabelableHolosignUIKey
{
    Key,
}

[Serializable, NetSerializable]
public sealed class LabelableHolosignChangedMessage(string description) : BoundUserInterfaceMessage
{
    public string Description { get; } = description;
}
