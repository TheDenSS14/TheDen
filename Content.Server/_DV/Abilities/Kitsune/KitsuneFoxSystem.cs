// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 M3739
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Polymorph.Components;
using Content.Server.Polymorph.Systems;
using Content.Shared._DV.Abilities.Kitsune;
using Content.Shared.Damage.Systems;
using Content.Shared.Stunnable;

namespace Content.Server._DV.Abilities.Kitsune;

public sealed class KitsuneFoxSystem : EntitySystem
{
    [Dependency] private readonly PolymorphSystem _polymorph = default!;
    [Dependency] private readonly SharedStaminaSystem _stamina = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<KitsuneFoxComponent, StunnedEvent>(OnStunned);
    }

    private void OnStunned(Entity<KitsuneFoxComponent> ent, ref StunnedEvent args)
    {
        if (!TryComp<PolymorphedEntityComponent>(ent, out var polymorph))
            return;

        _polymorph.Revert(ent.Owner);
    }
}
