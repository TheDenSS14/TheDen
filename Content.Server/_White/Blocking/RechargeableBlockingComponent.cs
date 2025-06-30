// SPDX-FileCopyrightText: 2025 BramvanZijp <56019239+BramvanZijp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._White.Blocking;

[RegisterComponent]
public sealed partial class RechargeableBlockingComponent : Component
{
    [DataField]
    public float DischargedRechargeRate = 1.33f;

    [DataField]
    public float ChargedRechargeRate = 2f;

    [ViewVariables]
    public bool Discharged;
}
