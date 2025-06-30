// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.WhiteDream.BloodCult.UI;

[Serializable, NetSerializable]
public enum NameSelectorUiKey
{
    Key
}

[Serializable, NetSerializable]
public sealed class NameSelectedMessage(string name)
    : BoundUserInterfaceMessage
{
    public string Name { get; } = name;
}
