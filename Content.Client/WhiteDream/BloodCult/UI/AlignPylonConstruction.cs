// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.WhiteDream.BloodCult.Components;
using Robust.Client.Placement;
using Robust.Client.Placement.Modes;
using Robust.Shared.Map;

namespace Content.Client.WhiteDream.BloodCult.UI;

public sealed class AlignPylonConstruction : SnapgridCenter
{
    [Dependency] private readonly IEntityManager _entityManager = default!;

    private readonly EntityLookupSystem _lookup;

    private const float PylonLookupRange = 10;

    public AlignPylonConstruction(PlacementManager pMan) : base(pMan)
    {
        IoCManager.InjectDependencies(this);
        _lookup = _entityManager.System<EntityLookupSystem>();
    }

    public override bool IsValidPosition(EntityCoordinates position)
    {
        return base.IsValidPosition(position) && !CheckForOtherPylons(position, PylonLookupRange);
    }

    private bool CheckForOtherPylons(EntityCoordinates coordinates, float range)
    {
        var entities = _lookup.GetEntitiesInRange(coordinates, range);
        return entities.Any(_entityManager.HasComponent<PylonComponent>);
    }
}
