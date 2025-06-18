// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared.Language.Components;

/// <summary>
///     Stores the current state of the languages the entity can speak and understand.
/// </summary>
/// <remarks>
///     All fields of this component are populated during a DetermineEntityLanguagesEvent.
///     They are not to be modified externally.
/// </remarks>
[RegisterComponent, NetworkedComponent]
public sealed partial class LanguageSpeakerComponent : Component
{
    public override bool SendOnlyToOwner => true;

    /// <summary>
    ///     The current language the entity uses when speaking.
    ///     Other listeners will hear the entity speak in this language.
    /// </summary>
    [DataField]
    public string CurrentLanguage = ""; // The language system will override it on mapinit

    /// <summary>
    ///     List of languages this entity can speak at the current moment.
    /// </summary>
    [DataField]
    public List<ProtoId<LanguagePrototype>> SpokenLanguages = [];

    /// <summary>
    ///     List of languages this entity can understand at the current moment.
    /// </summary>
    [DataField]
    public List<ProtoId<LanguagePrototype>> UnderstoodLanguages = [];

    [Serializable, NetSerializable]
    public sealed class State : ComponentState
    {
        public string CurrentLanguage = default!;
        public List<ProtoId<LanguagePrototype>> SpokenLanguages = default!;
        public List<ProtoId<LanguagePrototype>> UnderstoodLanguages = default!;
    }
}
