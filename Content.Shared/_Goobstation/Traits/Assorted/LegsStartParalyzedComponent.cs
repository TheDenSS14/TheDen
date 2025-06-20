// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2025 Will Oliver <willyangame76@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Traits.Assorted.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
/// Iterate through all the legs on the entity and make them paralyzed.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LegsStartParalyzedSystem))]
public sealed partial class LegsStartParalyzedComponent : Component
{
}
