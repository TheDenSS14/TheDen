// SPDX-FileCopyrightText: 2023 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Chat.Prototypes;

/// <summary>
///     Sounds collection for each <see cref="EmotePrototype"/>.
///     Different entities may use different sounds collections.
/// </summary>
[Prototype("emoteSounds"), Serializable, NetSerializable]
public sealed class EmoteSoundsPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Optional fallback sound that will play if collection
    ///     doesn't have specific sound for this emote id.
    /// </summary>
    [DataField("sound")]
    public SoundSpecifier? FallbackSound;

    /// <summary>
    ///     Optional audio params that will be applied to ALL sounds.
    ///     This will overwrite any params that may be set in sound specifiers.
    /// </summary>
    [DataField("params")]
    public AudioParams? GeneralParams;

    /// <summary>
    ///     Collection of emote prototypes and their sounds.
    /// </summary>
    [DataField("sounds", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<SoundSpecifier, EmotePrototype>))]
    public Dictionary<string, SoundSpecifier> Sounds = new();
}
