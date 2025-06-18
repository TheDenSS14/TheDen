// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Content.Shared.Chat;


namespace Content.Shared.Magic.Events;

/// <summary>
/// Adds provided Charge to the held wand
/// </summary>
public sealed partial class ChargeSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField(required: true)]
    public int Charge;

    [DataField]
    public string WandTag = "WizardWand";

    [DataField]
    public string? Speech { get; private set; }

    public InGameICChatType ChatType { get; } = InGameICChatType.Speak;
}
