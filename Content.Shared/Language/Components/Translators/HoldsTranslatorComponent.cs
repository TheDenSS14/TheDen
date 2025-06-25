// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Language.Components.Translators;

/// <summary>
///     Applied internally to the holder of an entity with [HandheldTranslatorComponent].
/// </summary>
[RegisterComponent]
public sealed partial class HoldsTranslatorComponent : Component
{
    [NonSerialized]
    public HashSet<Entity<HandheldTranslatorComponent>> Translators = new();
}
