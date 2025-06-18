// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Goobstation.Chemistry.HyposprayBlockNonMobInjection;

/// <summary>
/// For some reason if you set HyposprayComponent onlyAffectsMobs to true it would be able to draw from containers
/// even if injectOnly is also true. I don't want to modify HypospraySystem, so I made this component.
/// </summary>
[RegisterComponent]
public sealed partial class HyposprayBlockNonMobInjectionComponent : Component { }
