// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.IdentityManagement;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Verbs;

namespace Content.Shared.Item.ItemToggle;

/// <summary>
/// Adds a verb for toggling something with <see cref="ToggleVerbComponent"/>.
/// </summary>
public sealed class ToggleVerbSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _toggle = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ToggleVerbComponent, GetVerbsEvent<ActivationVerb>>(OnGetVerbs);
    }

    private void OnGetVerbs(Entity<ToggleVerbComponent> ent, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var name = Identity.Entity(ent, EntityManager);
        var user = args.User;
        args.Verbs.Add(new ActivationVerb()
        {
            Text = Loc.GetString(ent.Comp.Text, ("entity", name)),
            Act = () => _toggle.Toggle(ent.Owner, user)
        });
    }
}
