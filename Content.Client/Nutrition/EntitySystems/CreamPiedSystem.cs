// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using JetBrains.Annotations;
using Robust.Client.GameObjects;

namespace Content.Client.Nutrition.EntitySystems;

[UsedImplicitly]
public sealed class CreamPiedSystem : SharedCreamPieSystem
{
    [Dependency] private readonly SpriteSystem _spriteSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CreamPiedComponent, ComponentStartup>(OnComponentStartup);
    }

    // DEN change: Initialize cream pie sprites here so we don't have to change sprite layer orders jfc
    private void OnComponentStartup(Entity<CreamPiedComponent> ent, ref ComponentStartup args)
    {
        UpdateCreamPiedSprite(ent);
    }

    private void UpdateCreamPiedSprite(Entity<CreamPiedComponent> ent)
    {
        if (ent.Comp.Layer == null
            || !TryComp<SpriteComponent>(ent.Owner, out var sprite))
            return;

        _spriteSystem.LayerSetData((ent.Owner, sprite), ent.Comp.LayerKey, ent.Comp.Layer);
    }
    // End DEN
}
