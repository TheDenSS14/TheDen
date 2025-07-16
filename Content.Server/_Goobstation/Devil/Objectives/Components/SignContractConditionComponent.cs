// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Kai5
// SPDX-FileCopyrightText: 2025 Solstice
// SPDX-FileCopyrightText: 2025 SolsticeOfTheWinter
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Goobstation.Devil.Objectives.Systems;

namespace Content.Server._Goobstation.Devil.Objectives.Components;

[RegisterComponent, Access(typeof(DevilSystem), typeof(DevilObjectiveSystem))]

public sealed partial class SignContractConditionComponent : Component
{
    [DataField]
    public int ContractsSigned = 0;
}
