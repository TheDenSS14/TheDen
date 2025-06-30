// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.EmpFlashlight;

/// <summary>
///     Upon being triggered will EMP target.
/// </summary>
[RegisterComponent]
[Access(typeof(EmpOnHitSystem))]

public sealed partial class EmpOnHitComponent : Component
{
    [DataField]
    public float Range = 1.0f;

    [DataField]
    public float EnergyConsumption;

    [DataField]
    public float DisableDuration = 60f;
}
