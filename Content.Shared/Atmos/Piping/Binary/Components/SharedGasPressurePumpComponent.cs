// SPDX-FileCopyrightText: 2021 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Binary.Components
{
    [Serializable, NetSerializable]
    public enum GasPressurePumpUiKey
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class GasPressurePumpBoundUserInterfaceState : BoundUserInterfaceState
    {
        public string PumpLabel { get; }
        public float OutputPressure { get; }
        public bool Enabled { get; }

        public GasPressurePumpBoundUserInterfaceState(string pumpLabel, float outputPressure, bool enabled)
        {
            PumpLabel = pumpLabel;
            OutputPressure = outputPressure;
            Enabled = enabled;
        }
    }

    [Serializable, NetSerializable]
    public sealed class GasPressurePumpToggleStatusMessage : BoundUserInterfaceMessage
    {
        public bool Enabled { get; }

        public GasPressurePumpToggleStatusMessage(bool enabled)
        {
            Enabled = enabled;
        }
    }

    [Serializable, NetSerializable]
    public sealed class GasPressurePumpChangeOutputPressureMessage : BoundUserInterfaceMessage
    {
        public float Pressure { get; }

        public GasPressurePumpChangeOutputPressureMessage(float pressure)
        {
            Pressure = pressure;
        }
    }
}
