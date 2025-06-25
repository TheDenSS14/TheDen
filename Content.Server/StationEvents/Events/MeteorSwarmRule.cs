// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Morbo <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 T-Stalker <43253663+DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Spawners;

namespace Content.Server.StationEvents.Events
{
    public sealed class MeteorSwarmRule : StationEventSystem<MeteorSwarmRuleComponent>
    {
        [Dependency] private readonly SharedPhysicsSystem _physics = default!;

        protected override void Started(EntityUid uid, MeteorSwarmRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
        {
            base.Started(uid, component, gameRule, args);

            component.WaveCounter = RobustRandom.Next(component.MinimumWaves, component.MaximumWaves);
        }

        protected override void ActiveTick(EntityUid uid, MeteorSwarmRuleComponent component, GameRuleComponent gameRule, float frameTime)
        {
            if (component.WaveCounter <= 0)
            {
                ForceEndSelf(uid, gameRule);
                return;
            }

            component.Cooldown -= frameTime;

            if (component.Cooldown > 0f)
                return;

            component.WaveCounter--;

            component.Cooldown += (component.MaximumCooldown - component.MinimumCooldown) * RobustRandom.NextFloat() + component.MinimumCooldown;

            Box2? playableArea = null;
            var mapId = GameTicker.DefaultMap;

            var query = AllEntityQuery<MapGridComponent, TransformComponent>();
            while (query.MoveNext(out var gridId, out _, out var xform))
            {
                if (xform.MapID != mapId)
                    continue;

                var aabb = _physics.GetWorldAABB(gridId);
                playableArea = playableArea?.Union(aabb) ?? aabb;
            }

            if (playableArea == null)
            {
                ForceEndSelf(uid, gameRule);
                return;
            }

            var minimumDistance = (playableArea.Value.TopRight - playableArea.Value.Center).Length() + 50f;
            var maximumDistance = minimumDistance + 100f;

            var center = playableArea.Value.Center;

            for (var i = 0; i < component.MeteorsPerWave; i++)
            {
                var angle = new Angle(RobustRandom.NextFloat() * MathF.Tau);
                var offset = angle.RotateVec(new Vector2((maximumDistance - minimumDistance) * RobustRandom.NextFloat() + minimumDistance, 0));
                var spawnPosition = new MapCoordinates(center + offset, mapId);
                var meteor = Spawn("MeteorLarge", spawnPosition);
                var physics = EntityManager.GetComponent<PhysicsComponent>(meteor);
                _physics.SetBodyStatus(meteor, physics, BodyStatus.InAir);
                _physics.SetLinearDamping(meteor, physics, 0f);
                _physics.SetAngularDamping(meteor, physics, 0f);
                _physics.ApplyLinearImpulse(meteor, -offset.Normalized() * component.MeteorVelocity * physics.Mass, body: physics);
                _physics.ApplyAngularImpulse(
                    meteor,
                    physics.Mass * ((component.MaxAngularVelocity - component.MinAngularVelocity) * RobustRandom.NextFloat() + component.MinAngularVelocity),
                    body: physics);

                EnsureComp<TimedDespawnComponent>(meteor).Lifetime = 120f;
            }
        }
    }
}
