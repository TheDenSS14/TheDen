// SPDX-FileCopyrightText: 2025 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Stunnable;
using Content.Server.WhiteDream.BloodCult.Items.BaseAura;
using Content.Shared.Speech.Muting;
using Content.Shared.StatusEffect;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.WhiteDream.BloodCult.BloodCultist;
using Content.Shared.WhiteDream.BloodCult.Constructs;
using Content.Shared.WhiteDream.BloodCult.Spells;

namespace Content.Server.WhiteDream.BloodCult.Items.StunAura;

public sealed class StunAuraSystem : BaseAuraSystem<StunAuraComponent>
{
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
    [Dependency] private readonly StunSystem _stun = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StunAuraComponent, MeleeHitEvent>(OnMeleeHit);
    }

    private void OnMeleeHit(EntityUid uid, StunAuraComponent component, MeleeHitEvent args)
    {
        if (!args.HitEntities.Any())
            return;

        var target = args.HitEntities.First();
        if (uid == target || HasComp<BloodCultistComponent>(target) || HasComp<ConstructComponent>(target))
            return;

        RaiseLocalEvent(uid, new SpeakOnAuraUseEvent(args.User));

        _statusEffects.TryAddStatusEffect<MutedComponent>(target, "Muted", component.MuteDuration, true);
        _stun.TryParalyze(target, component.ParalyzeDuration, true);
        QueueDel(uid);
    }
}
