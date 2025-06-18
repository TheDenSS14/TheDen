// SPDX-FileCopyrightText: 2023 08A <git@08a.re>
// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Jezithyr <6192499+Jezithyr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 2024 Cojoke <83733158+Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Temperature;
using Robust.Server.GameObjects;

namespace Content.Server.IgnitionSource;

/// <summary>
/// This handles ignition, Jez basically coded this.
/// </summary>
public sealed class IgnitionSourceSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphere = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IgnitionSourceComponent, IsHotEvent>(OnIsHot);
        SubscribeLocalEvent<ItemToggleHotComponent, ItemToggledEvent>(OnItemToggle);
    }

    private void OnIsHot(Entity<IgnitionSourceComponent> ent, ref IsHotEvent args)
    {
        SetIgnited((ent.Owner, ent.Comp), args.IsHot);
    }
    private void OnItemToggle(Entity<ItemToggleHotComponent> ent, ref ItemToggledEvent args)
    {
        if (TryComp<IgnitionSourceComponent>(ent, out var comp))
            SetIgnited((ent.Owner, comp), args.Activated);
    }

    /// <summary>
    /// Simply sets the ignited field to the ignited param.
    /// </summary>
    public void SetIgnited(Entity<IgnitionSourceComponent?> ent, bool ignited = true)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.Ignited = ignited;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<IgnitionSourceComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var comp, out var xform))
        {
            if (!comp.Ignited)
                continue;

            if (xform.GridUid is { } gridUid)
            {
                var position = _transform.GetGridOrMapTilePosition(uid, xform);
                _atmosphere.HotspotExpose(gridUid, position, comp.Temperature, 50, uid, true);
            }
        }
    }
}
