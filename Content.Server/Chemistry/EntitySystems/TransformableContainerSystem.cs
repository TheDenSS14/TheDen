// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Ygg01 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chemistry.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.EntitySystems;

public sealed class TransformableContainerSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionsSystem = default!;
    [Dependency] private readonly MetaDataSystem _metadataSystem = default!;
    [Dependency] private readonly NameModifierSystem _nameMod = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TransformableContainerComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<TransformableContainerComponent, SolutionContainerChangedEvent>(OnSolutionChange);
        SubscribeLocalEvent<TransformableContainerComponent, RefreshNameModifiersEvent>(OnRefreshNameModifiers);
    }

    private void OnMapInit(Entity<TransformableContainerComponent> entity, ref MapInitEvent args)
    {
        var meta = MetaData(entity.Owner);
        if (string.IsNullOrEmpty(entity.Comp.InitialDescription))
        {
            entity.Comp.InitialDescription = meta.EntityDescription;
        }
    }

    private void OnSolutionChange(Entity<TransformableContainerComponent> entity, ref SolutionContainerChangedEvent args)
    {
        if (!_solutionsSystem.TryGetFitsInDispenser(entity.Owner, out _, out var solution))
            return;

        //Transform container into initial state when emptied
        if (entity.Comp.CurrentReagent != null && solution.Contents.Count == 0)
        {
            CancelTransformation(entity);
        }

        //the biggest reagent in the solution decides the appearance
        var reagentId = solution.GetPrimaryReagentId();

        //If biggest reagent didn't changed - don't change anything at all
        if (entity.Comp.CurrentReagent != null && entity.Comp.CurrentReagent.ID == reagentId?.Prototype)
        {
            return;
        }

        //Only reagents with spritePath property can change appearance of transformable containers!
        if (!string.IsNullOrWhiteSpace(reagentId?.Prototype)
            && _prototypeManager.TryIndex(reagentId.Value.Prototype, out ReagentPrototype? proto))
        {
            var metadata = MetaData(entity.Owner);
            _metadataSystem.SetEntityDescription(entity.Owner, proto.LocalizedDescription, metadata);
            entity.Comp.CurrentReagent = proto;
            entity.Comp.Transformed = true;
        }

        _nameMod.RefreshNameModifiers(entity.Owner);
    }

    private void OnRefreshNameModifiers(Entity<TransformableContainerComponent> entity, ref RefreshNameModifiersEvent args)
    {
        if (entity.Comp.CurrentReagent is { } currentReagent)
        {
            args.AddModifier("transformable-container-component-glass", priority: -1, ("reagent", currentReagent.LocalizedName));
        }
    }

    private void CancelTransformation(Entity<TransformableContainerComponent> entity)
    {
        entity.Comp.CurrentReagent = null;
        entity.Comp.Transformed = false;

        var metadata = MetaData(entity);

        _nameMod.RefreshNameModifiers(entity.Owner);

        if (!string.IsNullOrEmpty(entity.Comp.InitialDescription))
        {
            _metadataSystem.SetEntityDescription(entity.Owner, entity.Comp.InitialDescription, metadata);
        }
    }
}
