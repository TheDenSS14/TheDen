// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos;

namespace Content.Server._Goobstation.Temperature;

/// <summary>
/// Kills an entity when its temperature goes over a threshold.
/// </summary>
[RegisterComponent, Access(typeof(KillOnOverheatSystem))]
public sealed partial class KillOnOverheatComponent : Component
{
    [DataField]
    public float OverheatThreshold = Atmospherics.T0C + 110f;

    [DataField]
    public LocId OverheatPopup = "ipc-overheat-popup";
}
