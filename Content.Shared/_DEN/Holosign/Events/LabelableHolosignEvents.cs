// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Holosign.Events;

[Serializable, NetSerializable]
public enum LabelableHolosignUIKey
{
    Signs,
    Description,
}

[Serializable, NetSerializable]
public sealed class LabelableHolosignDescriptionMessage(string description, bool isNsfw) : BoundUserInterfaceMessage
{
    public string Description { get; } = description;
    public bool IsNsfw { get; } = isNsfw;
}

[Serializable, NetSerializable]
public sealed class LabelableHolosignSignChosen(int selection) : BoundUserInterfaceMessage
{
    public int Selection { get; } = selection;
}

[Serializable, NetSerializable]
public sealed class LabelableHolosignOpenOtherUI : BoundUserInterfaceMessage;