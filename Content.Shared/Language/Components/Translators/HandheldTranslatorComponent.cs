// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Language.Components.Translators;

/// <summary>
///   A translator that must be held in a hand or a pocket of an entity in order ot have effect.
/// </summary>
[RegisterComponent]
public sealed partial class HandheldTranslatorComponent : BaseTranslatorComponent
{
    /// <summary>
    ///   Whether interacting with this translator toggles it on and off.
    /// </summary>
    [DataField]
    public bool ToggleOnInteract = true;

    /// <summary>
    ///     If true, when this translator is turned on, the entities' current spoken language will be set
    ///     to the first new language added by this translator.
    /// </summary>
    /// <remarks>
    ///      This should generally be used for translators that translate speech between two languages.
    /// </remarks>
    [DataField]
    public bool SetLanguageOnInteract = true;
}
