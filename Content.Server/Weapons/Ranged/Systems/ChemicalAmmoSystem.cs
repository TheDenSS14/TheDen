// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Weapons.Ranged.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Weapons.Ranged.Events;
using System.Linq;

namespace Content.Server.Weapons.Ranged.Systems
{
    public sealed class ChemicalAmmoSystem : EntitySystem
    {
        [Dependency] private readonly SolutionContainerSystem _solutionContainerSystem = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<ChemicalAmmoComponent, AmmoShotEvent>(OnFire);
        }

        private void OnFire(Entity<ChemicalAmmoComponent> entity, ref AmmoShotEvent args)
        {
            if (!_solutionContainerSystem.TryGetSolution(entity.Owner, entity.Comp.SolutionName, out var ammoSoln, out var ammoSolution))
                return;

            var projectiles = args.FiredProjectiles;

            var projectileSolutionContainers = new List<(EntityUid, Entity<SolutionComponent>)>();
            foreach (var projectile in projectiles)
            {
                if (_solutionContainerSystem
                    .TryGetSolution(projectile, entity.Comp.SolutionName, out var projectileSoln, out _))
                {
                    projectileSolutionContainers.Add((projectile, projectileSoln.Value));
                }
            }

            if (!projectileSolutionContainers.Any())
                return;

            var solutionPerProjectile = ammoSolution.Volume * (1 / projectileSolutionContainers.Count);

            foreach (var (_, projectileSolution) in projectileSolutionContainers)
            {
                var solutionToTransfer = _solutionContainerSystem.SplitSolution(ammoSoln.Value, solutionPerProjectile);
                _solutionContainerSystem.TryAddSolution(projectileSolution, solutionToTransfer);
            }

            _solutionContainerSystem.RemoveAllSolution(ammoSoln.Value);
        }
    }
}
