// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chemistry.Components;
using Content.Client.Chemistry.UI;
using Content.Client.Items;
using Content.Shared.Chemistry.EntitySystems;

namespace Content.Client.Chemistry.EntitySystems;

/// <summary>
/// Wires up item status logic for <see cref="SolutionItemStatusComponent"/>.
/// </summary>
/// <seealso cref="SolutionStatusControl"/>
public sealed class SolutionItemStatusSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        Subs.ItemStatus<SolutionItemStatusComponent>(
            entity => new SolutionStatusControl(entity, EntityManager, _solutionContainerSystem));
    }
}
