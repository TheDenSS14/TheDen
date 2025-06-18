// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Actions;

/// <summary>
/// <see cref="ActionGrantComponent"/>
/// </summary>
public sealed class ActionGrantSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ActionGrantComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ActionGrantComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ItemActionGrantComponent, GetItemActionsEvent>(OnItemGet);
    }

    private void OnItemGet(Entity<ItemActionGrantComponent> ent, ref GetItemActionsEvent args)
    {
        if (!TryComp(ent.Owner, out ActionGrantComponent? grant))
            return;

        foreach (var action in grant.ActionEntities)
        {
            args.AddAction(action);
        }
    }

    private void OnMapInit(Entity<ActionGrantComponent> ent, ref MapInitEvent args)
    {
        foreach (var action in ent.Comp.Actions)
        {
            EntityUid? actionEnt = null;
            _actions.AddAction(ent.Owner, ref actionEnt, action);

            if (actionEnt != null)
                ent.Comp.ActionEntities.Add(actionEnt.Value);
        }
    }

    private void OnShutdown(Entity<ActionGrantComponent> ent, ref ComponentShutdown args)
    {
        foreach (var actionEnt in ent.Comp.ActionEntities)
        {
            _actions.RemoveAction(ent.Owner, actionEnt);
        }
    }
}
