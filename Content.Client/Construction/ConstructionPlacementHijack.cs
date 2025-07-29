// SPDX-FileCopyrightText: 2019 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2019 Silver
// SPDX-FileCopyrightText: 2019 ZelteHonor
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2023 08A
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 ArtisticRoomba
// SPDX-FileCopyrightText: 2025 chromiumboy
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Shared.Construction.Prototypes;
using Robust.Client.Placement;
using Robust.Client.Utility;
using Robust.Shared.Map;

namespace Content.Client.Construction
{
    public sealed class ConstructionPlacementHijack : PlacementHijack
    {
        private readonly ConstructionSystem _constructionSystem;
        private readonly ConstructionPrototype? _prototype;

        public ConstructionSystem? CurrentConstructionSystem { get { return _constructionSystem; } }
        public ConstructionPrototype? CurrentPrototype { get { return _prototype; } }

        public override bool CanRotate { get; }

        public ConstructionPlacementHijack(ConstructionSystem constructionSystem, ConstructionPrototype? prototype)
        {
            _constructionSystem = constructionSystem;
            _prototype = prototype;
            CanRotate = prototype?.CanRotate ?? true;
        }

        /// <inheritdoc />
        public override bool HijackPlacementRequest(EntityCoordinates coordinates)
        {
            if (_prototype != null)
            {
                var dir = Manager.Direction;
                _constructionSystem.SpawnGhost(_prototype, coordinates, dir);
            }
            return true;
        }

        /// <inheritdoc />
        public override bool HijackDeletion(EntityUid entity)
        {
            if (IoCManager.Resolve<IEntityManager>().HasComponent<ConstructionGhostComponent>(entity))
            {
                _constructionSystem.ClearGhost(entity.GetHashCode());
            }
            return true;
        }

        /// <inheritdoc />
        public override void StartHijack(PlacementManager manager)
        {
            base.StartHijack(manager);
            manager.CurrentTextures = _prototype?.Layers.Select(sprite => sprite.DirFrame0()).ToList();
        }
    }
}
