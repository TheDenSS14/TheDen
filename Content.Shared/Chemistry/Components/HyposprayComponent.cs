// SPDX-FileCopyrightText: 2024 Plykiya
// SPDX-FileCopyrightText: 2024 beck-thompson
// SPDX-FileCopyrightText: 2025 CyberLanos
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Audio;

namespace Content.Shared.Chemistry.Components;

/// <summary>
///     Component that allows an entity instantly transfer liquids by interacting with objects that have solutions.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HyposprayComponent : Component
{
    [DataField]
    public float MaxPressure = float.MaxValue;

    [DataField]
    public float InjectTime;

    /// <summary>
    ///     Solution that will be used by hypospray for injections.
    /// </summary>
    [DataField]
    public string SolutionName = "hypospray";

    /// <summary>
    ///     Amount of the units that will be transfered.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 TransferAmount = FixedPoint2.New(5);

    /// <summary>
    ///     Sound that will be played when injecting.
    /// </summary>
    [DataField]
    public SoundSpecifier InjectSound = new SoundPathSpecifier("/Audio/Items/hypospray.ogg");

    /// <summary>
    /// Decides whether you can inject everything or just mobs.
    /// When you can only affect mobs, you're capable of drawing from beakers.
    /// </summary>
    [AutoNetworkedField]
    [DataField(required: true)]
    public bool OnlyAffectsMobs;

    /// <summary>
    /// Whether or not the hypospray is able to draw from containers or if it's a single use
    /// device that can only inject.
    /// </summary>
    [DataField]
    public bool InjectOnly;

    /// <summary>
    /// Whether the injecting entity needs hands for the operation.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool NeedHands = true;

    /// <summary>
    /// Whether or not the hypospray injects it's entire capacity on use.
    /// Used by Jet Injectors.
    /// </summary>
    [DataField]
    public bool InjectMaxCapacity;

    /// <summary>
    /// Whether or not the hypospray can inject chitinids.
    /// Used by Jet Injectors.
    /// </summary>
    [DataField]
    public bool BypassBlockInjection = true;
}
