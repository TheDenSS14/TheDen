// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Maps;
using Robust.Server.Console;
using Robust.Shared.Utility;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;

namespace Content.Server.Maps;

/// <inheritdoc />
public sealed class GridDraggingSystem : SharedGridDraggingSystem
{
    [Dependency] private readonly IConGroupController _admin = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    private readonly HashSet<ICommonSession> _draggers = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<GridDragRequestPosition>(OnRequestDrag);
        SubscribeNetworkEvent<GridDragVelocityRequest>(OnRequestVelocity);
    }

    public bool IsEnabled(ICommonSession session) => _draggers.Contains(session);

    public void Toggle(ICommonSession session)
    {
        if (session is not { } pSession)
            return;

        DebugTools.Assert(_admin.CanCommand(pSession, CommandName));

        // Weird but it's a toggle
        if (_draggers.Add(session))
        {

        }
        else
        {
            _draggers.Remove(session);
        }

        RaiseNetworkEvent(new GridDragToggleMessage()
        {
            Enabled = _draggers.Contains(session),
        }, session.Channel);
    }

    private void OnRequestVelocity(GridDragVelocityRequest ev, EntitySessionEventArgs args)
    {
        var grid = GetEntity(ev.Grid);

        if (args.SenderSession is not { } playerSession ||
            !_admin.CanCommand(playerSession, CommandName) ||
            !Exists(grid) ||
            Deleted(grid))
        {
            return;
        }

        var gridBody = Comp<PhysicsComponent>(grid);
        _physics.SetLinearVelocity(grid, ev.LinearVelocity, body: gridBody);
        _physics.SetAngularVelocity(grid, 0f, body: gridBody);
    }

    private void OnRequestDrag(GridDragRequestPosition msg, EntitySessionEventArgs args)
    {
        var grid = GetEntity(msg.Grid);

        if (args.SenderSession is not { } playerSession ||
            !_admin.CanCommand(playerSession, CommandName) ||
            !Exists(grid) ||
            Deleted(grid))
        {
            return;
        }

        var gridXform = Transform(grid);

        gridXform.WorldPosition = msg.WorldPosition;
    }
}
