// SPDX-FileCopyrightText: 2022 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos;
using Content.Shared.Atmos;

namespace Content.Server.Medical.Components;

[RegisterComponent]
public sealed partial class CryoPodAirComponent : Component
{
    /// <summary>
    /// Local air buffer that will be mixed with the pipenet, if one exists, per tick.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("gasMixture")]
    public GasMixture Air { get; set; } = new GasMixture(1000f);
}
