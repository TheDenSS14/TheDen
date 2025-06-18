// SPDX-FileCopyrightText: 2025 Rouge2t7 <81053047+Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Construction.Completions;
using Content.Server.Explosion.Components;
using Content.Server.Explosion.EntitySystems;


namespace Content.Server._Goobstation.Explosion.EntitySystems;


public sealed partial class GoobTriggerSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DeleteParentOnTriggerComponent, TriggerEvent>(HandleDeleteParentTrigger);
    }

    private void HandleDeleteParentTrigger(Entity<DeleteParentOnTriggerComponent> entity, ref TriggerEvent args)
    {

        var uid = entity.Owner;

        if (!TryComp<TransformComponent>(uid, out var userXform))
            return;

        if (userXform.ParentUid == userXform.GridUid || userXform.ParentUid == userXform.MapUid)
            return;

        EntityManager.QueueDeleteEntity(userXform.ParentUid);
        args.Handled = true;
    }

}
