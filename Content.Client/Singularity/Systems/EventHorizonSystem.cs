// SPDX-FileCopyrightText: 2022 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Singularity.EntitySystems;
using Content.Shared.Singularity.Components;

namespace Content.Client.Singularity.EntitySystems;

/// <summary>
/// The client-side version of <see cref="SharedEventHorizonSystem"/>.
/// Primarily manages <see cref="EventHorizonComponent"/>s.
/// Exists to make relevant signal handlers (ie: <see cref="SharedEventHorizonSystem.OnPreventCollide"/>) work on the client.
/// </summary>
public sealed class EventHorizonSystem : SharedEventHorizonSystem
{}
