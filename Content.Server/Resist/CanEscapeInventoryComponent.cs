// SPDX-FileCopyrightText: 2022 JustinTime <41876089+JustinTether@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;

namespace Content.Server.Resist;

[RegisterComponent]
public sealed partial class CanEscapeInventoryComponent : Component
{
    /// <summary>
    ///     Base doafter length for uncontested breakouts.
    /// </summary>
    [DataField]
    public float BaseResistTime = 5f;

    public bool IsEscaping => DoAfter != null;

    [DataField("doAfter")]
    public DoAfterId? DoAfter;

    /// <summary>
    ///     Action to cancel inventory escape.
    /// </summary>
    [DataField]
    public EntityUid? EscapeCancelAction;
}
