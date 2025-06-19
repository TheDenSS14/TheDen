// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class GlimmerMonitorUiState : BoundUserInterfaceState
{
    public List<int> GlimmerValues;

    public GlimmerMonitorUiState(List<int> glimmerValues)
    {
        GlimmerValues = glimmerValues;
    }
}

[Serializable, NetSerializable]
public sealed class GlimmerMonitorSyncMessageEvent : CartridgeMessageEvent
{
    public GlimmerMonitorSyncMessageEvent()
    {}
}
