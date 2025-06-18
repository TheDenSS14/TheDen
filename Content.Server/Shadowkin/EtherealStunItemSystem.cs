// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Interaction.Events;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Shadowkin;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Stacks;

namespace Content.Server.Shadowkin;

public sealed class EtherealStunItemSystem : EntitySystem
{
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedStackSystem _sharedStackSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<EtherealStunItemComponent, UseInHandEvent>(OnUseInHand);
    }

    private void OnUseInHand(EntityUid uid, EtherealStunItemComponent component, UseInHandEvent args)
    {
        foreach (var ent in _lookup.GetEntitiesInRange(uid, component.Radius))
        {
            if (!TryComp<EtherealComponent>(ent, out var ethereal)
                || !ethereal.CanBeStunned)
                continue;

            RemComp(ent, ethereal);

            if (TryComp<StaminaComponent>(ent, out var stamina))
                _stamina.TakeStaminaDamage(ent, stamina.CritThreshold, stamina, ent);
        }

        if (!component.DeleteOnUse)
            return;

        if (TryComp<StackComponent>(uid, out var stack))
            _sharedStackSystem.Use(uid, 1, stack);
        else
            QueueDel(uid);
    }
}
