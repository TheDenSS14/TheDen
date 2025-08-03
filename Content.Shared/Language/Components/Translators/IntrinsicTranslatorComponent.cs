// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Language.Components.Translators;

/// <summary>
///   A translator attached to an entity that translates its speech.
///   An example is a translator implant that allows the speaker to speak another language.
/// </summary>
[RegisterComponent, Virtual]
public partial class IntrinsicTranslatorComponent : Translators.BaseTranslatorComponent
{
}
