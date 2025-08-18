// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2023 KP <13428215+nok-ko@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Damage.Components;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Damage;
using Content.Shared.Throwing;
using Content.Shared._DV.Chemistry.Systems; // DeltaV - Beergoggles enable safe throw
using Content.Shared.Nutrition.Components; // DeltaV - Beergoggles enable safe throw

namespace Content.Server.Damage.Systems
{
    public sealed class DamageOnLandSystem : EntitySystem
    {
        [Dependency] private readonly DamageableSystem _damageableSystem = default!;
        [Dependency] private readonly SafeSolutionThrowerSystem _safesolthrower = default!; // DeltaV - Beergoggles enable safe throw

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<DamageOnLandComponent, LandEvent>(DamageOnLand);
            SubscribeLocalEvent<DamageOnLandComponent, AttemptPacifiedThrowEvent>(OnAttemptPacifiedThrow);
        }

        /// <summary>
        /// Prevent Pacified entities from throwing damaging items.
        /// </summary>
        private void OnAttemptPacifiedThrow(Entity<DamageOnLandComponent> ent, ref AttemptPacifiedThrowEvent args)
        {
            // Allow healing projectiles, forbid any that do damage:
            if (ent.Comp.Damage.AnyPositive())
            {
                args.Cancel("pacified-cannot-throw");
            }
        }

        private void DamageOnLand(EntityUid uid, DamageOnLandComponent component, ref LandEvent args)
        {
            // DeltaV - start of Beergoggles enable safe throw
            if (args.User is { } user && HasComp<DrinkComponent>(uid))
            {
                if (_safesolthrower.GetSafeThrow(user))
                    return;
            }
            // DeltaV - end of Beergoggles enable safe throw
            _damageableSystem.TryChangeDamage(uid, component.Damage, component.IgnoreResistances);
        }
    }
}
