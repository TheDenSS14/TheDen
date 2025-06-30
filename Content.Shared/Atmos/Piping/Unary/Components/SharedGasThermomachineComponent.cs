// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Ilya246 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 2023 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.Components;

[Serializable, NetSerializable]
public sealed record GasThermoMachineData(float EnergyDelta);

[Serializable]
[NetSerializable]
public enum ThermomachineUiKey : byte
{
    Key
}

[Serializable]
[NetSerializable]
public sealed class GasThermomachineToggleMessage : BoundUserInterfaceMessage
{
}

[Serializable]
[NetSerializable]
public sealed class GasThermomachineChangeTemperatureMessage : BoundUserInterfaceMessage
{
    public float Temperature { get; }

    public GasThermomachineChangeTemperatureMessage(float temperature)
    {
        Temperature = temperature;
    }
}
