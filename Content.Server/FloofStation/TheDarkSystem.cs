// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.GameTicking.Events;
using Content.Server.Shuttles.Components;
using Content.Shared.Shuttles.Components;
using Robust.Server.GameObjects;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;

namespace Content.Server.FloofStation;

public sealed class TheDarkSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly MapLoaderSystem _loader = default!;
    //
    // public override void Initialize()
    // {
    //     base.Initialize();
    //
    //     // SubscribeLocalEvent<RoundStartingEvent>(SetupTheDark);
    // }
    //
    // private void SetupTheDark(RoundStartingEvent ev)
    // {
    //     var mapId = _mapManager.CreateMap();
    //     _mapManager.AddUninitializedMap(mapId);
    //
    //     if (!_loader.TryLoad(mapId, "/Maps/Floof/hideout.yml", out var uids))
    //         return;
    //
    //     foreach (var id in uids)
    //     {
    //         EnsureComp<PreventPilotComponent>(id);
    //     }
    //
    //     _mapManager.DoMapInitialize(mapId);
    // }
}
