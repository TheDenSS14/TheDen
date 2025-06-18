// SPDX-FileCopyrightText: 2021 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Stunnable.Systems;
using Content.Shared.Whitelist;

namespace Content.Server.Stunnable.Components;

/// <summary>
///     Adds stun when it collides with an entity
/// </summary>
[RegisterComponent, Access(typeof(StunOnCollideSystem))]
public sealed partial class StunOnCollideComponent : Component
{
    // TODO: Can probably predict this.

    [DataField]
    public TimeSpan StunAmount = TimeSpan.FromSeconds(5);

    [DataField]
    public TimeSpan KnockdownAmount = TimeSpan.FromSeconds(5);

    [DataField]
    public TimeSpan SlowdownAmount = TimeSpan.FromSeconds(10);

    [DataField]
    public float WalkSpeedMultiplier = 1f;

    [DataField]
    public float RunSpeedMultiplier = 1f;

    /// <summary>
    ///     Fixture we track for the collision.
    /// </summary>
    [DataField]
    public string FixtureId = "projectile";

    /// <summary>
    ///     Entities excluded from collision check.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
