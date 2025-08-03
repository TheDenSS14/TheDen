// SPDX-FileCopyrightText: 2022 Moony
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(VentClogRule))]
public sealed partial class VentCritterSpawnLocationComponent : Component
{
    [DataField]
    public bool CanSpawn { get; set; } = true;
}
