// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Body.Events;
using Content.Server.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared._Shitmed.Body.Organ;
using Content.Server._Shitmed.DelayedDeath;

namespace Content.Server._Shitmed.Body.Organ;

public sealed class HeartSystem : EntitySystem
{
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HeartComponent, OrganAddedToBodyEvent>(HandleAddition);
        SubscribeLocalEvent<HeartComponent, OrganRemovedFromBodyEvent>(HandleRemoval);
    }

    private void HandleRemoval(EntityUid uid, HeartComponent _, ref OrganRemovedFromBodyEvent args)
    {
        if (TerminatingOrDeleted(uid) || TerminatingOrDeleted(args.OldBody))
            return;

        // TODO: Add some form of very violent bleeding effect.
        EnsureComp<DelayedDeathComponent>(args.OldBody);
    }

    private void HandleAddition(EntityUid uid, HeartComponent _, ref OrganAddedToBodyEvent args)
    {
        if (TerminatingOrDeleted(uid) || TerminatingOrDeleted(args.Body))
            return;

        if (_bodySystem.TryGetBodyOrganComponents<BrainComponent>(args.Body, out var _))
            RemComp<DelayedDeathComponent>(args.Body);
    }
    // Shitmed-End
}
