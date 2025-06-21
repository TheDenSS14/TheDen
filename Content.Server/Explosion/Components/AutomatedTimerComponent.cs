// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Explosion.Components;

/// <summary>
///     Disallows starting the timer by hand, must be stuck or triggered by a system using <c>StartTimer</c>.
/// </summary>
[RegisterComponent]
public sealed partial class AutomatedTimerComponent : Component
{
}
