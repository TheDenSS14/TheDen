// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

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
    public ProtoId<EmoteSoundsPrototype> AdditionalSounds { get; set; }
}
