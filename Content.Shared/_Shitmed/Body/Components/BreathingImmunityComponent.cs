// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Shitmed.Body.Components;

/// <summary>
///     Disables a mobs need for air when this component is added.
///     It will neither breathe nor take airloss damage.
/// </summary>
[RegisterComponent]
public sealed partial class BreathingImmunityComponent : Component;
