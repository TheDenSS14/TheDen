// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 SixplyDev <einlichen@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Abilities.Psionics;
using Content.Shared.Actions.Events;
using Content.Shared.Shadowkin;
using Content.Shared.Physics;
using Content.Shared.Popups;
using Content.Shared.Maps;
using Robust.Server.GameObjects;

namespace Content.Server.Abilities.Psionics
{
    public sealed class DarkSwapSystem : EntitySystem
    {
        [Dependency] private readonly SharedPsionicAbilitiesSystem _psionics = default!;
        [Dependency] private readonly SharedPopupSystem _popup = default!;
        [Dependency] private readonly PhysicsSystem _physics = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<DarkSwapActionEvent>(OnPowerUsed);
        }

        private void OnPowerUsed(DarkSwapActionEvent args)
        {
            if (TryComp<EtherealComponent>(args.Performer, out var ethereal))
            {
                var tileref = Transform(args.Performer).Coordinates.GetTileRef();
                if (tileref != null
                && _physics.GetEntitiesIntersectingBody(args.Performer, (int) CollisionGroup.Impassable).Count > 0)
                {
                    _popup.PopupEntity(Loc.GetString("revenant-in-solid"), args.Performer, args.Performer);
                    return;
                }

                if (_psionics.OnAttemptPowerUse(args.Performer, "DarkSwap"))
                {
                    SpawnAtPosition("ShadowkinShadow", Transform(args.Performer).Coordinates);
                    SpawnAtPosition("EffectFlashShadowkinDarkSwapOff", Transform(args.Performer).Coordinates);
                    RemComp(args.Performer, ethereal);
                    args.Handled = true;
                }
            }
            else if (_psionics.OnAttemptPowerUse(args.Performer, "DarkSwap"))
            {
                var newethereal = EnsureComp<EtherealComponent>(args.Performer);
                newethereal.Darken = true;

                SpawnAtPosition("ShadowkinShadow", Transform(args.Performer).Coordinates);
                SpawnAtPosition("EffectFlashShadowkinDarkSwapOn", Transform(args.Performer).Coordinates);

                args.Handled = true;
            }

            if (args.Handled)
                _psionics.LogPowerUsed(args.Performer, "DarkSwap");
        }
    }
}
