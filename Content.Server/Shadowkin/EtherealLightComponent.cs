// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Shadowkin;

[RegisterComponent]
public sealed partial class EtherealLightComponent : Component
{
    public EntityUid AttachedEntity = EntityUid.Invalid;

    public float OldRadius = 0f;

    public bool OldRadiusEdited = false;

    public float OldEnergy = 0f;

    public bool OldEnergyEdited = false;
}