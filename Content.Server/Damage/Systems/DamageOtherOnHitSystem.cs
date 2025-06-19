// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Silver <silvertorch5@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Camera;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.Systems;
using Content.Shared.Database;
using Content.Shared.Effects;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee;
using Content.Server.Weapons.Melee;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Damage.Systems
{
    public sealed class DamageOtherOnHitSystem : SharedDamageOtherOnHitSystem
    {
        [Dependency] private readonly DamageExamineSystem _damageExamine = default!;
        [Dependency] private readonly SharedPopupSystem _popup = default!;
        [Dependency] private readonly StaminaSystem _stamina = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<StaminaComponent, BeforeThrowEvent>(OnBeforeThrow, after: [typeof(PacificationSystem)]);
            SubscribeLocalEvent<DamageOtherOnHitComponent, DamageExamineEvent>(OnDamageExamine, after: [typeof(MeleeWeaponSystem)]);
        }

        private void OnBeforeThrow(EntityUid uid, StaminaComponent component, ref BeforeThrowEvent args)
        {
            if (args.Cancelled || !TryComp<DamageOtherOnHitComponent>(args.ItemUid, out var damage))
                return;

            if (component.CritThreshold - component.StaminaDamage <= damage.StaminaCost)
            {
                args.Cancelled = true;
                _popup.PopupEntity(Loc.GetString("throw-no-stamina", ("item", args.ItemUid)), uid, uid);
                return;
            }

            _stamina.TakeStaminaDamage(uid, damage.StaminaCost, component, visual: false);
        }

        private void OnDamageExamine(EntityUid uid, DamageOtherOnHitComponent component, ref DamageExamineEvent args)
        {
            _damageExamine.AddDamageExamine(args.Message, GetDamage(uid, component, user: args.User), Loc.GetString("damage-throw"));

            if (component.StaminaCost == 0)
                return;

            var staminaCostMarkup = FormattedMessage.FromMarkupOrThrow(
                Loc.GetString("damage-stamina-cost",
                ("type", Loc.GetString("damage-throw")), ("cost", Math.Round(component.StaminaCost, 2).ToString("0.##"))));
            args.Message.PushNewline();
            args.Message.AddMessage(staminaCostMarkup);
        }
    }
}
