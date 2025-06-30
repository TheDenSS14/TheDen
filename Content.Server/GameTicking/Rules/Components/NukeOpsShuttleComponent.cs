// SPDX-FileCopyrightText: 2023 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2023 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Tags grid as nuke ops shuttle
/// </summary>
[RegisterComponent]
public sealed partial class NukeOpsShuttleComponent : Component
{
    [DataField]
    public EntityUid AssociatedRule;
}
