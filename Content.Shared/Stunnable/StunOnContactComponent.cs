// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Stunnable;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedStunSystem))]
public sealed partial class StunOnContactComponent : Component
{
    /// <summary>
    /// The fixture the entity must collide with to be stunned
    /// </summary>
    [DataField]
    public string FixtureId = "fix";

    /// <summary>
    /// The duration of the stun.
    /// </summary>
    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(5);

    [DataField]
    public EntityWhitelist Blacklist = new();

    /// <summary>
    ///     If the entity has or exceeds this speed when contacting then stun.
    /// </summary>
    [DataField]
    public float TriggerSpeed = 0;
}
