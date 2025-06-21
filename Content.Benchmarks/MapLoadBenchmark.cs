// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Content.IntegrationTests;
using Content.IntegrationTests.Pair;
using Content.Server.Maps;
using Robust.Shared;
using Robust.Shared.Analyzers;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Benchmarks;

[Virtual]
public class MapLoadBenchmark
{
    private TestPair _pair = default!;
    private MapLoaderSystem _mapLoader = default!;
    private SharedMapSystem _mapSys = default!;

    [GlobalSetup]
    public void Setup()
    {
        ProgramShared.PathOffset = "../../../../";
        PoolManager.Startup();

        _pair = PoolManager.GetServerClient().GetAwaiter().GetResult();
        var server = _pair.Server;

        Paths = server.ResolveDependency<IPrototypeManager>()
            .EnumeratePrototypes<GameMapPrototype>()
            .ToDictionary(x => x.ID, x => x.MapPath.ToString());

        _mapLoader = server.ResolveDependency<IEntitySystemManager>().GetEntitySystem<MapLoaderSystem>();
        _mapSys = server.ResolveDependency<IEntitySystemManager>().GetEntitySystem<SharedMapSystem>();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _pair.DisposeAsync();
        PoolManager.Shutdown();
    }

    public static readonly string[] MapsSource = { "Empty", "Box", "Aspid", "Bagel", "Dev", "CentComm", "Atlas", "Core", "TestTeg", "Saltern", "Packed", "Omega", "Cluster", "Gemini", "Reach", "Origin", "Meta", "Marathon", "Europa", "MeteorArena", "Fland", "Barratry" };

    [ParamsSource(nameof(MapsSource))]
    public string Map;

    public Dictionary<string, string> Paths;
    private MapId _mapId;

    [Benchmark]
    public async Task LoadMap()
    {
        var mapPath = new ResPath(Paths[Map]);
        var server = _pair.Server;
        await server.WaitPost(() =>
        {
            var success = _mapLoader.TryLoadMap(mapPath, out var map, out _);
            if (!success)
                throw new Exception("Map load failed");
            _mapId = map.Value.Comp.MapId;
        });
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        var server = _pair.Server;
        server.WaitPost(() => _mapSys.DeleteMap(_mapId))
            .Wait();
    }
}
