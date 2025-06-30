// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Humanoid;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.WhiteDream.BloodCult.BloodCultist;
using Content.Shared.WhiteDream.BloodCult.Constructs;

namespace Content.Server.WhiteDream.BloodCult.Runes;

public sealed partial class CultRuneBaseSystem
{
    [Dependency] private readonly PullingSystem _pulling = default!;

    /// <summary>
    ///     Gets all cultists/constructs near rune.
    /// </summary>
    public HashSet<EntityUid> GatherCultists(EntityUid rune, float range)
    {
        var runeTransform = Transform(rune);
        var entities = _lookup.GetEntitiesInRange(runeTransform.Coordinates, range);
        entities.RemoveWhere(entity => !HasComp<BloodCultistComponent>(entity) && !HasComp<ConstructComponent>(entity));
        return entities;
    }

    /// <summary>
    ///     Gets all the humanoids near rune.
    /// </summary>
    /// <param name="rune">The rune itself.</param>
    /// <param name="range">Radius for a lookup.</param>
    /// <param name="exlude">Filter to exlude from return.</param>
    public HashSet<Entity<HumanoidAppearanceComponent>> GetTargetsNearRune(
        EntityUid rune,
        float range,
        Predicate<Entity<HumanoidAppearanceComponent>>? exlude = null
    )
    {
        var runeTransform = Transform(rune);
        var possibleTargets = _lookup.GetEntitiesInRange<HumanoidAppearanceComponent>(runeTransform.Coordinates, range);
        if (exlude != null)
            possibleTargets.RemoveWhere(exlude);

        return possibleTargets;
    }

    /// <summary>
    ///     Is used to stop target from pulling/being pulled before teleporting them.
    /// </summary>
    public void StopPulling(EntityUid target)
    {
        if (TryComp(target, out PullableComponent? pullable) && pullable.BeingPulled)
            _pulling.TryStopPull(target, pullable);

        // I wish there was a better way to do it
        if (_pulling.TryGetPulledEntity(target, out var pulling))
            _pulling.TryStopPull(pulling.Value);
    }
}
