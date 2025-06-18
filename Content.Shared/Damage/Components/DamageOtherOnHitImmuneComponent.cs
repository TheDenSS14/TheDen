// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Damage.Components;

/// <summary>
/// This is used for entities that are immune to getting hit by DamageOtherOnHit, and getting embedded from EmbeddableProjectile.
/// </summary>
[RegisterComponent]
public sealed partial class DamageOtherOnHitImmuneComponent : Component {}
