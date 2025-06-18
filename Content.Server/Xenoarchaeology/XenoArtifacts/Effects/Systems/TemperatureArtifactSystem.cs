// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Components;
using Content.Server.Xenoarchaeology.XenoArtifacts.Events;
using Content.Shared.Atmos;
using Robust.Server.GameObjects;

namespace Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Systems;

public sealed class TemperatureArtifactSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
    [Dependency] private readonly TransformSystem _transformSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TemperatureArtifactComponent, ArtifactActivatedEvent>(OnActivate);
    }

    private void OnActivate(EntityUid uid, TemperatureArtifactComponent component, ArtifactActivatedEvent args)
    {
        var transform = Transform(uid);

        var center = _atmosphereSystem.GetContainingMixture(uid, false, true);
        if (center == null)
            return;
        UpdateTileTemperature(component, center);

        if (component.AffectAdjacentTiles && transform.GridUid != null)
        {
            var enumerator = _atmosphereSystem.GetAdjacentTileMixtures(transform.GridUid.Value,
                _transformSystem.GetGridOrMapTilePosition(uid, transform), excite: true);

            while (enumerator.MoveNext(out var mixture))
            {
                UpdateTileTemperature(component, mixture);
            }
        }
    }

    private void UpdateTileTemperature(TemperatureArtifactComponent component, GasMixture environment)
    {
        var dif = component.TargetTemperature - environment.Temperature;
        var absDif = Math.Abs(dif);
        var step = Math.Min(absDif, component.SpawnTemperature);
        environment.Temperature += dif > 0 ? step : -step;
    }
}
