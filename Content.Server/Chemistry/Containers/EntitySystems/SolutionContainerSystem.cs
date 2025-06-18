// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Server.Chemistry.Containers.EntitySystems;

[Obsolete("This is being depreciated. Use SharedSolutionContainerSystem instead!")]
public sealed partial class SolutionContainerSystem : SharedSolutionContainerSystem
{
    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name)
        => EnsureSolution(entity, name, out _);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, out bool existed)
        => EnsureSolution(entity, name, FixedPoint2.Zero, out existed);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, FixedPoint2 maxVol, out bool existed)
        => EnsureSolution(entity, name, maxVol, null, out existed);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, FixedPoint2 maxVol, Solution? prototype, out bool existed)
    {
        EnsureSolution(entity, name, maxVol, prototype, out existed, out var solution);
        return solution!;//solution is only ever null on the client, so we can suppress this
    }

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Entity<SolutionComponent> EnsureSolutionEntity(
        Entity<SolutionContainerManagerComponent?> entity,
        string name,
        FixedPoint2 maxVol,
        Solution? prototype,
        out bool existed)
    {
        EnsureSolutionEntity(entity, name, out existed, out var solEnt, maxVol, prototype);
        return solEnt!.Value;//solEnt is only ever null on the client, so we can suppress this
    }
}
