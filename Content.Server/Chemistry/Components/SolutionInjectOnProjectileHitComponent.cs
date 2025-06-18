// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Chemistry.Components;

/// <summary>
/// Used for projectile entities that should try to inject a
/// contained solution into a target when they hit it.
/// </summary>
[RegisterComponent]
public sealed partial class SolutionInjectOnProjectileHitComponent : BaseSolutionInjectOnEventComponent { }
