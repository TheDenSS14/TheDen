// SPDX-FileCopyrightText: 2026 Alex C
//
// SPDX-License-Identifier: MIT

using Content.Shared._DEN.ReagentProduction.Prototypes;
using Content.Shared.Traits;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;


namespace Content.Shared._DEN.ReagentProduction.Systems;


public sealed partial class ReagentProductionTraitFunction
{
    [UsedImplicitly]
    public sealed partial class TraitAddReagentProduction : TraitFunction
    {
        [DataField, AlwaysPushInheritance,]
        public List<ProtoId<ReagentProductionTypePrototype>> ReagentProductionTypes = [];

        public override void OnPlayerSpawn(
            EntityUid uid,
            IComponentFactory factory,
            IEntityManager entityManager,
            ISerializationManager serializationManager
        )
        {
            var prototype = IoCManager.Resolve<IPrototypeManager>();
            var reagentProduction = entityManager.System<ReagentProductionSystem>();

            foreach (var productionTypeId in ReagentProductionTypes)
                if (prototype.TryIndex(productionTypeId, out var productionType))
                    reagentProduction.AddProductionType(uid, productionType);
        }
    }
}
