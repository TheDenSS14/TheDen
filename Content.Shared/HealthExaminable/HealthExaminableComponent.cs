// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared.HealthExaminable;

[RegisterComponent, Access(typeof(HealthExaminableSystem))]
public sealed partial class HealthExaminableComponent : Component
{
    // <summary>
    //     The thresholds for determining the examine text for certain amounts of damage.
    //     These are calculated as a percentage of the entity's critical threshold.
    // </summary>
    public List<FixedPoint2> Thresholds = new()
        { FixedPoint2.New(0.10), FixedPoint2.New(0.25), FixedPoint2.New(0.50), FixedPoint2.New(0.75) };

    [DataField(required: true)]
    public HashSet<ProtoId<DamageTypePrototype>> ExaminableTypes = default!;

    /// <summary>
    ///     Health examine text is automatically generated through creating loc string IDs, in the form:
    ///     `health-examine-[prefix]-[type]-[threshold]`
    ///     This part determines the prefix.
    /// </summary>
    [DataField]
    public string LocPrefix = "carbon";
}
