// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 brainfood1183 <113240905+brainfood1183@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 Whisper <121047731+QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Cuffs.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedCuffableSystem))]
public sealed partial class HandcuffComponent : Component
{
    /// <summary>
    /// Whether or not the entity can rot when cuffed (for spooder cocoon)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool NoRot = false;

    /// <summary>
    ///     The time it takes to cuff an entity.
    /// </summary>
    [DataField]
    public float CuffTime = 3.5f;

    /// <summary>
    ///     The time it takes to uncuff an entity.
    /// </summary>
    [DataField]
    public float UncuffTime = 3.5f;

    /// <summary>
    ///     The time it takes for a cuffed entity to uncuff itself.
    /// </summary>
    [DataField]
    public float BreakoutTime = 15f;

    /// <summary>
    ///     If an entity being cuffed is stunned, this amount of time is subtracted from the time it takes to add/remove their cuffs.
    /// </summary>
    [DataField]
    public float StunBonus = 2f;

    /// <summary>
    ///     Will the cuffs break when removed?
    /// </summary>
    [DataField]
    public bool BreakOnRemove;

    /// <summary>
    ///     Will the cuffs break when removed?
    /// </summary>
    [DataField]
    public EntProtoId? BrokenPrototype;

    /// <summary>
    /// Whether or not these cuffs are in the process of being removed.
    /// Used simply to prevent spawning multiple <see cref="BrokenPrototype"/>.
    /// </summary>
    [DataField]
    public bool Removing;

    /// <summary>
    /// Whether the cuffs are currently being used to cuff someone.
    /// We need the extra information for when the virtual item is deleted because that can happen when you simply stop
    /// pulling them on the ground.
    /// </summary>
    [DataField]
    public bool Used;

    /// <summary>
    ///     The path of the RSI file used for the player cuffed overlay.
    /// </summary>
    [DataField]
    public string? CuffedRSI = "Objects/Misc/handcuffs.rsi";

    /// <summary>
    ///     The iconstate used with the RSI file for the player cuffed overlay.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? BodyIconState = "body-overlay";

    /// <summary>
    /// An opptional color specification for <see cref="BodyIconState"/>
    /// </summary>
    [DataField]
    public Color Color = Color.White;

    [DataField]
    public SoundSpecifier StartCuffSound = new SoundPathSpecifier("/Audio/Items/Handcuffs/cuff_start.ogg");

    [DataField]
    public SoundSpecifier EndCuffSound = new SoundPathSpecifier("/Audio/Items/Handcuffs/cuff_end.ogg");

    [DataField]
    public SoundSpecifier StartBreakoutSound = new SoundPathSpecifier("/Audio/Items/Handcuffs/cuff_breakout_start.ogg");

    [DataField]
    public SoundSpecifier StartUncuffSound = new SoundPathSpecifier("/Audio/Items/Handcuffs/cuff_takeoff_start.ogg");

    [DataField]
    public SoundSpecifier EndUncuffSound = new SoundPathSpecifier("/Audio/Items/Handcuffs/cuff_takeoff_end.ogg");

    /// <summary>
    ///     Acts as a two-state option for handcuff speed. When true, handcuffs will be easier to get out of if you are larger than average. Representing the use of strength to break things like zipties.
    ///     When false, handcuffs are easier to get out of if you are smaller than average, representing the use of dexterity to slip the cuffs.
    /// </summary>
    [DataField]
    public bool UncuffEasierWhenLarge = false;
}

/// <summary>
/// Event fired on the User when the User attempts to uncuff the Target.
/// Should generate popups on the User.
/// </summary>
[ByRefEvent]
public record struct UncuffAttemptEvent(EntityUid User, EntityUid Target)
{
    public readonly EntityUid User = User;
    public readonly EntityUid Target = Target;
    public bool Cancelled = false;
}
