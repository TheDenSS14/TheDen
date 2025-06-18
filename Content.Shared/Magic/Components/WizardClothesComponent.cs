// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Magic.Components;

/// <summary>
/// The <see cref="SharedMagicSystem"/> checks this if a spell requires wizard clothes
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedMagicSystem))]
public sealed partial class WizardClothesComponent : Component;
