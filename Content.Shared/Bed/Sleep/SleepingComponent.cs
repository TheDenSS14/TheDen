// SPDX-FileCopyrightText: 2022 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Bed.Sleep;

/// <summary>
/// Added to entities when they go to sleep.
/// </summary>
[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause(Dirty = true)]
public sealed partial class SleepingComponent : Component
{
    /// <summary>
    /// How much damage of any type it takes to wake this entity.
    /// </summary>
    [DataField]
    public FixedPoint2 WakeThreshold = FixedPoint2.New(2);

    /// <summary>
    ///     Cooldown time between users hand interaction.
    /// </summary>
    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(1f);

    [DataField]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan CooldownEnd;

    /// <summary>
    ///     The moment this entity went to sleep. Initialized on MapInit.
    /// </summary>
    [DataField]
    public TimeSpan SleepingSince;

    [DataField]
    [AutoNetworkedField]
    public EntityUid? WakeAction;

    /// <summary>
    /// Sound to play when another player attempts to wake this entity.
    /// </summary>
    [DataField]
    public SoundSpecifier WakeAttemptSound = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg")
    {
        Params = AudioParams.Default.WithVariation(0.05f)
    };
}
