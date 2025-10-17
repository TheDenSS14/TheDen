// SPDX-FileCopyrightText: 2022 Francesco
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2025 bigsantino1
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Dataset;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;


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

    /// <summary>
    ///     The fluent string prefix to use when picking a random suffix
    ///     This is only active for those who have the sleeping component
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> ForceSaySleepDataset = "ForceSaySleepDataset";
}
