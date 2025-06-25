// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

/// <summary>
/// This is used to keep track of hallucinated entities to remove effects when event ends
/// </summary>
[RegisterComponent, Access(typeof(MassHallucinationsRule))]
public sealed partial class MassHallucinationsComponent : Component
{
}
