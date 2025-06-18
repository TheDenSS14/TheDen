// SPDX-FileCopyrightText: 2024 GreaseMonk <1354802+GreaseMonk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Sound.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Audio;

/// <summary>
/// Toggles <see cref="AmbientSoundComponent"/> and <see cref="SpamEmitSoundComponent"/> off when this entity's MobState isn't Alive.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SoundWhileAliveComponent : Component;
