// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 juniwoofs <180479595+juniwoofs@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(PirateRadioSpawnRule))]
public sealed partial class PirateRadioSpawnRuleComponent : Component
{
    [DataField]
    public List<string> PirateRadioShuttlePath { get; private set; } = new()
    {
        "Maps/TheDen/LPO.yml", // Floofstation
    };

    [DataField]
    public EntityUid? AdditionalRule;

    [DataField]
    public int DebrisCount { get; set; }

    [DataField]
    public float MinimumDistance { get; set; } = 750f;

    [DataField]
    public float MaximumDistance { get; set; } = 1250f;

    [DataField]
    public float MinimumDebrisDistance { get; set; } = 150f;

    [DataField]
    public float MaximumDebrisDistance { get; set; } = 250f;

    [DataField]
    public float DebrisMinimumOffset { get; set; } = 50f;

    [DataField]
    public float DebrisMaximumOffset { get; set; } = 100f;
}
