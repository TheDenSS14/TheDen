// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._EE.PressureDestructible.Components;
using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;

namespace Content.Server._EE.PressureDestructible.EntitySystems;


/// <summary>
/// This handles destroying entities based on pressure if it has a PressureDestructible component
/// </summary>
public sealed class PressureDestructibleSystem : EntitySystem
{
    [Dependency] private TransformSystem _transform = default!;
    [Dependency] private AtmosphereSystem _atmosphere = default!;
    [Dependency] private DamageableSystem _damageable = default!;
    [Dependency] private IGameTiming _gameTiming = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityManager.EntityQueryEnumerator<PressureDestructibleComponent, DamageableComponent>();
        while (query.MoveNext(out var uid, out var pressureDestructible, out var damageable))
        {
            if (_gameTiming.CurTime < pressureDestructible.NextUpdate || pressureDestructible.MaxPressureDifferential == 0)
                continue;

            pressureDestructible.NextUpdate = _gameTiming.CurTime + pressureDestructible.CheckInterval;

            if (uid == EntityUid.Invalid || !Exists(uid))
                continue;

            var grid = _transform.GetGrid(uid);
            var currentTile = _transform.GetGridOrMapTilePosition(uid);

            if (grid == null || grid == EntityUid.Invalid || !Exists(grid))
                continue;

            var hasAtmos = _atmosphere.TryGetTileAtmosphere((grid.Value, null), currentTile, out _);

            if (!hasAtmos)
                continue;

            var adjacentTiles = new HashSet<TileAtmosphere>();
            var directionsToCheck = new[] { AtmosDirection.North, AtmosDirection.East, AtmosDirection.South, AtmosDirection.West };

            foreach (var direction in directionsToCheck)
            {
                var adjacentTile = currentTile.Offset(direction);

                if (!_atmosphere.TryGetTileAtmosphere(grid.Value, adjacentTile, out var adjacentAtmos))
                    continue;

                adjacentTiles.Add(adjacentAtmos);
            }

            var greatestDifference = 0f;
            TileAtmosphere? largestPressureTile = null;

            foreach (var tileAtmos in adjacentTiles)
            {
                var largestPressure = largestPressureTile?.Air?.Pressure ?? 0;
                var tileMix = _atmosphere.GetTileMixture(grid.Value, null, tileAtmos.GridIndices, true);
                var tilePressure = tileMix?.Pressure!;

                if (tilePressure == null)
                    return;

                var difference = MathF.Abs(largestPressure - (float) tilePressure);

                if (tilePressure == 0)
                    continue;

                if (difference > greatestDifference)
                    greatestDifference = difference;

                if (tilePressure > largestPressure)
                    largestPressureTile = tileAtmos;
            }

            if (greatestDifference < pressureDestructible.MaxPressureDifferential || HasComp<PressureDestructibleImmuneComponent>(uid))
                continue;

            var damageSpecifier = damageable.Damage;
            var currentDamage = damageSpecifier["Blunt"];

            damageSpecifier.DamageDict["Blunt"] = currentDamage + pressureDestructible.Damage;
            _damageable.SetDamage(uid, damageable, damageSpecifier);
        }
    }
}
