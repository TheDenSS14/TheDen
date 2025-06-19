// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos;
namespace Content.Server._Lavaland.Pressure;

[RegisterComponent]
public sealed partial class PressureDamageChangeComponent : Component
{
    [DataField]
    public float LowerBound = 0;

    [DataField]
    public float UpperBound = Atmospherics.OneAtmosphere * 0.5f;

    [DataField]
    public bool ApplyWhenInRange = false;

    [DataField]
    public float AppliedModifier = 0.33f;
}
