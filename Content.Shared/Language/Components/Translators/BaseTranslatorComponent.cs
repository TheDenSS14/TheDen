// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Language.Components.Translators;

public abstract partial class BaseTranslatorComponent : Component
{
    /// <summary>
    ///   The list of additional languages this translator allows the wielder to speak.
    /// </summary>
    [DataField("spoken")]
    public List<ProtoId<LanguagePrototype>> SpokenLanguages = new();

    /// <summary>
    ///   The list of additional languages this translator allows the wielder to understand.
    /// </summary>
    [DataField("understood")]
    public List<ProtoId<LanguagePrototype>> UnderstoodLanguages = new();

    /// <summary>
    ///   The languages the wielding MUST know in order for this translator to have effect.
    ///   The field [RequiresAllLanguages] indicates whether all of them are required, or just one.
    /// </summary>
    [DataField("requires")]
    public List<ProtoId<LanguagePrototype>> RequiredLanguages = new();

    /// <summary>
    ///   If true, the wielder must understand all languages in [RequiredLanguages] to speak [SpokenLanguages],
    ///   and understand all languages in [RequiredLanguages] to understand [UnderstoodLanguages].
    ///
    ///   Otherwise, at least one language must be known (or the list must be empty).
    /// </summary>
    [DataField("requiresAll")]
    public bool RequiresAllLanguages = false;

    [DataField("enabled")]
    public bool Enabled = true;
}
