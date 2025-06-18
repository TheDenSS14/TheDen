// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Content.Shared.Chat;
using Robust.Shared.Prototypes;

namespace Content.Shared.Magic.Events;

public sealed partial class InstantSpawnSpellEvent : InstantActionEvent, ISpeakSpell
{
    /// <summary>
    /// What entity should be spawned.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Prototype;

    [DataField]
    public bool PreventCollideWithCaster = true;

    [DataField]
    public string? Speech { get; private set; }

    public InGameICChatType ChatType { get; } = InGameICChatType.Speak;

    /// <summary>
    /// Gets the targeted spawn positons; may lead to multiple entities being spawned.
    /// </summary>
    [DataField]
    public MagicInstantSpawnData PosData = new TargetCasterPos();
}
