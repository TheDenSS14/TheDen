// SPDX-FileCopyrightText: 2023 Adrian16199 <144424013+Adrian16199@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Tools;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Tools.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Nyanotrasen.Abilities.Oni;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Containers;

namespace Content.Server.Abilities.Oni
{
    public sealed class OniSystem : SharedOniSystem
    {
        [Dependency] private readonly ToolSystem _toolSystem = default!;
        [Dependency] private readonly GunSystem _gunSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<OniComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
            SubscribeLocalEvent<OniComponent, EntRemovedFromContainerMessage>(OnEntRemoved);
            SubscribeLocalEvent<MeleeWeaponComponent, GetMeleeDamageEvent>(OnGetMeleeDamage);
            SubscribeLocalEvent<HeldByOniComponent, TakeStaminaDamageEvent>(OnStamHit);
        }

        private void OnEntInserted(EntityUid uid, OniComponent component, EntInsertedIntoContainerMessage args)
        {
            var heldComp = EnsureComp<HeldByOniComponent>(args.Entity);
            heldComp.Holder = uid;

            if (TryComp<ToolComponent>(args.Entity, out var tool) && _toolSystem.HasQuality(args.Entity, "Prying", tool))
                _toolSystem.SetSpeedModifier((args.Entity, tool), tool.SpeedModifier * 1.66f);

            if (_gunSystem.TryGetGun(args.Entity, out _, out var gun))
            {
                gun.MinAngle *= 15f;
                gun.AngleIncrease *= 15f;
                gun.MaxAngle *= 15f;
            }
        }

        private void OnEntRemoved(EntityUid uid, OniComponent component, EntRemovedFromContainerMessage args)
        {
            if (TryComp<ToolComponent>(args.Entity, out var tool) && _toolSystem.HasQuality(args.Entity, "Prying", tool))
                _toolSystem.SetSpeedModifier((args.Entity, tool), tool.SpeedModifier / 1.66f);

            if (_gunSystem.TryGetGun(args.Entity, out _, out var gun))
            {
                gun.MinAngle /= 15f;
                gun.AngleIncrease /= 15f;
                gun.MaxAngle /= 15f;
            }

            RemComp<HeldByOniComponent>(args.Entity);
        }

        private void OnGetMeleeDamage(EntityUid uid, MeleeWeaponComponent component, ref GetMeleeDamageEvent args)
        {
            if (!TryComp<OniComponent>(args.User, out var oni))
                return;

            args.Modifiers.Add(oni.MeleeModifiers);
        }

        private void OnStamHit(EntityUid uid, HeldByOniComponent component, TakeStaminaDamageEvent args)
        {
            if (!TryComp<OniComponent>(component.Holder, out var oni))
                return;

            args.Multiplier *= oni.StamDamageMultiplier;
        }
    }
}
