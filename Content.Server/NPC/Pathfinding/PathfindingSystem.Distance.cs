// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.NPC;

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    public float EuclideanDistance(PathPoly start, PathPoly end)
    {
        var (dx, dy) = GetDiff(start, end);
        return MathF.Sqrt((dx * dx + dy * dy));
    }

    public float ManhattanDistance(PathPoly start, PathPoly end)
    {
        var (dx, dy) = GetDiff(start, end);
        return dx + dy;
    }

    public float OctileDistance(PathPoly start, PathPoly end)
    {
        var (dx, dy) = GetDiff(start, end);
        return dx + dy + (1.41f - 2) * Math.Min(dx, dy);
    }

    private Vector2 GetDiff(PathPoly start, PathPoly end)
    {
        var startPos = start.Box.Center;
        var endPos = end.Box.Center;

        if (end.GraphUid != start.GraphUid)
        {
            if (!TryComp<TransformComponent>(start.GraphUid, out var startXform) ||
                !TryComp<TransformComponent>(end.GraphUid, out var endXform))
            {
                return Vector2.Zero;
            }

            endPos = Vector2.Transform(Vector2.Transform(endPos, endXform.WorldMatrix), startXform.InvWorldMatrix);
        }

        // TODO: Numerics when we changeover.
        var diff = startPos - endPos;
        var ab = Vector2.Abs(diff);
        return ab;
    }
}
