// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Language;

// <summary>
//     Signifies that this entity can speak and understand any language.
//     Applies to such entities as ghosts.
// </summary>
[RegisterComponent]
public sealed partial class UniversalLanguageSpeakerComponent : Component
{
    [DataField]
    public bool Enabled = true;
}
