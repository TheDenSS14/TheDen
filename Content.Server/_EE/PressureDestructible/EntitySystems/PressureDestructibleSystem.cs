using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;


namespace Content.Server._EE.PressureDestructible;


/// <summary>
/// This handles destroying objects based on pressure if it has a PressureDestructible component
/// </summary>
public sealed class PressureDestructibleSystem : EntitySystem
{
    [Dependency] private TransformSystem _transform = default!;
    [Dependency] private AtmosphereSystem _atmosphere = default!;
    [Dependency] private DamageableSystem _damageable = default!;
    [Dependency] private IGameTiming _gameTiming = default!;

    private FixedPoint2 _maxDamage = FixedPoint2.New(50);

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityManager.EntityQueryEnumerator<PressureDestructibleComponent, DamageableComponent>();

        while (query.MoveNext(out var uid, out var pressureDestructible, out var damageable))
        {
            if (_gameTiming.CurTime < pressureDestructible.NextUpdate)
                continue;

            pressureDestructible.NextUpdate = _gameTiming.CurTime + pressureDestructible.CheckInterval;

            if (uid == EntityUid.Invalid || !Exists(uid))
                continue;

            var grid = _transform.GetGrid(uid);
            var currentTile = _transform.GetGridOrMapTilePosition(uid);

            if (grid == null || grid == EntityUid.Invalid || !Exists(grid))
                continue;

            var hasAtmos = _atmosphere.TryGetTileAtmosphere((grid.Value, null), currentTile, out var currentTileAtmos);
            var neighborTiles = _atmosphere.GetAdjacentTileAtmospheres((grid.Value, null), currentTile);

            if (!hasAtmos)
                continue;

            while (neighborTiles.MoveNext(out var tileAtmos, out var index))
            {
                var currentDirection = (AtmosDirection) index;
                var oppositeDirection = currentDirection.GetOpposite();
                var oppositeIndex = (int) oppositeDirection;

                var oppositeTile = currentTileAtmos?.AdjacentTiles[oppositeIndex];
                var currentPressure = tileAtmos.Air?.Pressure ?? 0;
                var oppositePressure = oppositeTile?.Air?.Pressure ?? 0;
                var difference = MathF.Abs(currentPressure - oppositePressure);

                if (difference < pressureDestructible.MaxPressureDifferential)
                    continue;

                var damageMultiplier = difference != 0 ? difference / pressureDestructible.MaxPressureDifferential : 1f;
                var damage = _maxDamage * damageMultiplier;
                var damageSpecifier = damageable.Damage;
                var currentDamage = damageSpecifier["Blunt"];

                damageSpecifier.DamageDict["Blunt"] = currentDamage + damage * FixedPoint2.New(damageMultiplier);
                _damageable.SetDamage(uid, damageable, damageSpecifier);
            }
        }
    }
}
