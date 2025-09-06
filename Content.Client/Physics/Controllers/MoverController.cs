// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Plykiya <58439124+Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.CCVar;
using Content.Shared.Friction;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Systems;
using Robust.Client.GameObjects;
using Robust.Client.Physics;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Client.Physics.Controllers
{
    public sealed class MoverController : SharedMoverController
    {
        [Dependency] private readonly IConfigurationManager _config = default!;
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<RelayInputMoverComponent, LocalPlayerAttachedEvent>(OnRelayPlayerAttached);
            SubscribeLocalEvent<RelayInputMoverComponent, LocalPlayerDetachedEvent>(OnRelayPlayerDetached);
            SubscribeLocalEvent<InputMoverComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
            SubscribeLocalEvent<InputMoverComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

            SubscribeLocalEvent<InputMoverComponent, UpdateIsPredictedEvent>(OnUpdatePredicted);
            SubscribeLocalEvent<MovementRelayTargetComponent, UpdateIsPredictedEvent>(OnUpdateRelayTargetPredicted);
            SubscribeLocalEvent<PullableComponent, UpdateIsPredictedEvent>(OnUpdatePullablePredicted);

            Subs.CVar(_config, CCVars.DefaultWalk, _ => RaiseNetworkEvent(new UpdateInputCVarsMessage()));
        }

        private void OnUpdatePredicted(Entity<InputMoverComponent> entity, ref UpdateIsPredictedEvent args)
        {
            // Enable prediction if an entity is controlled by the player
            if (entity.Owner == _playerManager.LocalEntity)
                args.IsPredicted = true;
        }

        private void OnUpdateRelayTargetPredicted(Entity<MovementRelayTargetComponent> entity, ref UpdateIsPredictedEvent args)
        {
            if (entity.Comp.Source == _playerManager.LocalEntity)
                args.IsPredicted = true;
        }

        private void OnUpdatePullablePredicted(Entity<PullableComponent> entity, ref UpdateIsPredictedEvent args)
        {
            // Enable prediction if an entity is being pulled by the player.
            // Disable prediction if an entity is being pulled by some non-player entity.

            if (entity.Comp.Puller == _playerManager.LocalEntity)
                args.IsPredicted = true;
            else if (entity.Comp.Puller != null)
                args.BlockPrediction = true;

            // TODO recursive pulling checks?
            // What if the entity is being pulled by a vehicle controlled by the player?
        }

        private void OnRelayPlayerAttached(Entity<RelayInputMoverComponent> entity, ref LocalPlayerAttachedEvent args)
        {
            Physics.UpdateIsPredicted(entity.Owner);
            Physics.UpdateIsPredicted(entity.Comp.RelayEntity);
            if (MoverQuery.TryGetComponent(entity.Comp.RelayEntity, out var inputMover))
                SetMoveInput((entity.Comp.RelayEntity, inputMover), MoveButtons.None);
        }

        private void OnRelayPlayerDetached(Entity<RelayInputMoverComponent> entity, ref LocalPlayerDetachedEvent args)
        {
            Physics.UpdateIsPredicted(entity.Owner);
            Physics.UpdateIsPredicted(entity.Comp.RelayEntity);
            if (MoverQuery.TryGetComponent(entity.Comp.RelayEntity, out var inputMover))
                SetMoveInput((entity.Comp.RelayEntity, inputMover), MoveButtons.None);
        }

        private void OnPlayerAttached(Entity<InputMoverComponent> entity, ref LocalPlayerAttachedEvent args)
        {
            SetMoveInput(entity, MoveButtons.None);
        }

        private void OnPlayerDetached(Entity<InputMoverComponent> entity, ref LocalPlayerDetachedEvent args)
        {
            SetMoveInput(entity, MoveButtons.None);
        }

        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            if (_playerManager.LocalEntity is not {Valid: true} player)
                return;

            if (RelayQuery.TryGetComponent(player, out var relayMover))
                HandleClientsideMovement(relayMover.RelayEntity, frameTime);

            HandleClientsideMovement(player, frameTime);
        }

        private void HandleClientsideMovement(EntityUid player, float frameTime)
        {
            if (!MoverQuery.TryGetComponent(player, out var mover) ||
                !XformQuery.TryGetComponent(player, out var xform))
            {
                return;
            }

            var physicsUid = player;
            PhysicsComponent? body;
            var xformMover = xform;

            if (mover.ToParent && RelayQuery.HasComponent(xform.ParentUid))
            {
                if (!PhysicsQuery.TryGetComponent(xform.ParentUid, out body) ||
                    !XformQuery.TryGetComponent(xform.ParentUid, out xformMover))
                {
                    return;
                }

                physicsUid = xform.ParentUid;
            }
            else if (!PhysicsQuery.TryGetComponent(player, out body))
            {
                return;
            }

            // Server-side should just be handled on its own so we'll just do this shizznit
            HandleMobMovement(
                player,
                mover,
                physicsUid,
                body,
                xformMover,
                frameTime);
        }

        protected override bool CanSound()
        {
            return _timing is { IsFirstTimePredicted: true, InSimulation: true };
        }
    }
}
