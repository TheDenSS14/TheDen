// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Construction;
// using Content.Client.WhiteDream.BloodCult.UI;
using Content.Shared.Construction.Prototypes;
using Content.Shared.RadialSelector;
using Content.Shared.ShortConstruction;
using Robust.Client.Placement;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.ShortConstruction;

public sealed class ShortConstructionSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IPlacementManager _placement = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    [Dependency] private readonly ConstructionSystem _construction = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShortConstructionComponent, RadialSelectorSelectedMessage>(OnItemRecieved);
    }

    private void OnItemRecieved(Entity<ShortConstructionComponent> ent, ref RadialSelectorSelectedMessage args)
    {
        if (!_proto.TryIndex(args.SelectedItem, out ConstructionPrototype? prototype) ||
            !_gameTiming.IsFirstTimePredicted)
            return;

        if (prototype.Type == ConstructionType.Item)
        {
            _construction.TryStartItemConstruction(prototype.ID);
            return;
        }

        var hijack = new ConstructionPlacementHijack(_construction, prototype);

        _placement.BeginPlacing(new PlacementInformation
            {
                IsTile = false,
                PlacementOption = prototype.PlacementMode
            },
            hijack);
    }
}
