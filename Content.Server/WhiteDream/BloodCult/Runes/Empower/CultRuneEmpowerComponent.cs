// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.WhiteDream.BloodCult.Runes.Empower;

[RegisterComponent]
public sealed partial class CultRuneEmpowerComponent : Component
{
    [DataField]
    public string ComponentToGive = "BloodCultEmpowered";
}
