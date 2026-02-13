// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Chat.Prototypes;
using Content.Shared.Humanoid.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Vocal;


/// <summary>
/// This is used for imitating species noises.
/// </summary>
[RegisterComponent]
public sealed partial class AdditionalVocalSoundsComponent : Component
{
    [DataField]
    public HashSet<ProtoId<EmoteSoundsPrototype>> AdditionalSounds { get; set; } = new();

    [DataField]
    public ProtoId<EmoteSoundsPrototype>? ReplacesDefaultSounds { get; set; }
}
