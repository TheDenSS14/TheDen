// SPDX-FileCopyrightText: 2022 Acruid
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2025 ArtisticRoomba
// SPDX-FileCopyrightText: 2025 chromiumboy
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Robust.Server.GameObjects;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.EntitySystems;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.Piping.EntitySystems;

public sealed partial class AtmosPipeAppearanceSystem : SharedAtmosPipeAppearanceSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly MapSystem _map = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PipeAppearanceComponent, NodeGroupsRebuilt>(OnNodeUpdate);
    }

    private void OnNodeUpdate(EntityUid uid, PipeAppearanceComponent component, ref NodeGroupsRebuilt args)
    {
        UpdateAppearance(args.NodeOwner);
    }

    private void UpdateAppearance(EntityUid uid, AppearanceComponent? appearance = null, NodeContainerComponent? container = null,
        TransformComponent? xform = null)
    {
        if (!Resolve(uid, ref appearance, ref container, ref xform, false))
            return;

        if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
            return;

        var numberOfPipeLayers = GetNumberOfPipeLayers(uid, out var atmosPipeLayers);

        // get connected entities
        var anyPipeNodes = false;
        HashSet<(EntityUid, AtmosPipeLayer)> connected = new();

        foreach (var node in container.Nodes.Values)
        {
            if (node is not PipeNode)
                continue;

            anyPipeNodes = true;

            foreach (var connectedNode in node.ReachableNodes)
            {
                if (connectedNode is PipeNode { } pipeNode)
                    connected.Add((connectedNode.Owner, pipeNode.CurrentPipeLayer));
            }
        }

        if (!anyPipeNodes)
            return;

        // find the cardinal directions of any connected entities
        var connectedDirections = new PipeDirection[numberOfPipeLayers];
        Array.Fill(connectedDirections, PipeDirection.None);

        var tile = _map.TileIndicesFor(xform.GridUid.Value, grid, xform.Coordinates);

        foreach (var (neighbour, pipeLayer) in connected)
        {
            var pipeIndex = (int)pipeLayer;

            if (pipeIndex >= numberOfPipeLayers)
                continue;

            var otherTile = _map.TileIndicesFor(xform.GridUid.Value, grid, Transform(neighbour).Coordinates);
            var pipeLayerDirections = connectedDirections[pipeIndex];

            pipeLayerDirections |= (otherTile - tile) switch
            {
                (0, 1) => PipeDirection.North,
                (0, -1) => PipeDirection.South,
                (1, 0) => PipeDirection.East,
                (-1, 0) => PipeDirection.West,
                _ => PipeDirection.None
            };

            connectedDirections[pipeIndex] = pipeLayerDirections;
        }

        // Convert the pipe direction array into a single int for serialization
        var netConnectedDirections = 0;

        for (var i = numberOfPipeLayers - 1; i >= 0; i--)
            netConnectedDirections += (int)connectedDirections[i] << (PipeDirectionHelpers.PipeDirections * i);

        _appearance.SetData(uid, PipeVisuals.VisualState, netConnectedDirections, appearance);
    }
}
