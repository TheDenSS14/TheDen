// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LordEclipse <106132477+LordEclipse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Alert;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Shared.Nutrition.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(ThirstSystem))]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class ThirstComponent : Component
{
    // Base stuff
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("baseDecayRate")]
    [AutoNetworkedField]
    public float BaseDecayRate = 150.0f / (40 * 60); // One tier every 40 minutes

    /// <summary>
    /// A flat multiplier applied to BaseDecayRate.
    /// This shouldn't change, ideally; this is supposed to make species hunger rates more intuitive to code.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DecayRateMultiplier = 1.0f;

    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float ActualDecayRate;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public ThirstThreshold CurrentThirstThreshold;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public ThirstThreshold LastThirstThreshold;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("startingThirst")]
    [AutoNetworkedField]
    public float CurrentThirst = -1f;

    /// <summary>
    /// The time when the hunger will update next.
    /// </summary>
    [DataField("nextUpdateTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan NextUpdateTime;

    /// <summary>
    /// The time between each update.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public TimeSpan UpdateRate = TimeSpan.FromSeconds(1);

    [DataField("thresholds")]
    [AutoNetworkedField]
    public Dictionary<ThirstThreshold, float> ThirstThresholds = new()
    {
        {ThirstThreshold.OverHydrated, 600.0f},
        {ThirstThreshold.Okay, 450.0f},
        {ThirstThreshold.Thirsty, 300.0f},
        {ThirstThreshold.Parched, 150.0f},
        {ThirstThreshold.Dead, 0.0f},
    };

    [DataField]
    public ProtoId<AlertCategoryPrototype> ThirstyCategory = "Thirst";

    public static readonly Dictionary<ThirstThreshold, ProtoId<AlertPrototype>> ThirstThresholdAlertTypes = new()
    {
        {ThirstThreshold.Thirsty, "Thirsty"},
        {ThirstThreshold.Parched, "Parched"},
        {ThirstThreshold.Dead, "Parched"},
    };

    /// <summary>
    /// A dictionary relating ThirstThreshold to how much they modify <see cref="BaseDecayRate"/>.
    /// </summary>
    [DataField("hungerThresholdDecayModifiers", customTypeSerializer: typeof(DictionarySerializer<ThirstThreshold, float>))]
    [AutoNetworkedField]
    public Dictionary<ThirstThreshold, float> ThirstThresholdDecayModifiers = new()
    {
        {ThirstThreshold.OverHydrated, 1.2f},
        {ThirstThreshold.Okay, 1.0f },
        {ThirstThreshold.Thirsty, 0.8f },
        {ThirstThreshold.Parched, 0.6f},
        {ThirstThreshold.Dead, 0f}
    };
}

[Flags]
public enum ThirstThreshold : byte
{
    // Hydrohomies
    Dead = 0,
    Parched = 1 << 0,
    Thirsty = 1 << 1,
    Okay = 1 << 2,
    OverHydrated = 1 << 3,
}
