// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Mail.Components;

/// <summary>
///     A placeholder for another entity, spawned when dropped or placed in someone's hands.
///     Useful for storing instant effect entities, e.g. smoke, in the mail.
/// </summary>
[RegisterComponent]
public sealed partial class DelayedItemComponent : Component
{
    /// <summary>
    ///     The entity to replace this when opened or dropped.
    /// </summary>
    [DataField]
    public string Item = "None";
}
