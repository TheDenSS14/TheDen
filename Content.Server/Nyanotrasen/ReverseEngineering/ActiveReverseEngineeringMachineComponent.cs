// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Server.ReverseEngineering;

[RegisterComponent]
public sealed partial class ActiveReverseEngineeringMachineComponent : Component
{
    /// <summary>
    /// When did the scanning start?
    /// </summary>
    [DataField("startTime", customTypeSerializer: typeof(TimespanSerializer))]
    public TimeSpan StartTime;

    /// <summary>
    /// What is being scanned?
    /// </summary>
    [ViewVariables]
    public EntityUid Item;
}
