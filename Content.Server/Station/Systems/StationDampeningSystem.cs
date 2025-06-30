// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 neuPanda <chriseparton@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Shuttles.Components;
using Content.Server.Station.Events;
using Content.Shared.Physics;

namespace Content.Server.Station.Systems;

public sealed class StationDampeningSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<StationPostInitEvent>(OnInitStation);
    }

    private void OnInitStation(ref StationPostInitEvent ev)
    {
        foreach (var grid in ev.Station.Comp.Grids)
        {
            // If the station grid doesn't have defined dampening, give it a small dampening by default
            // This will ensure cargo tech pros won't fling the station 1000 megaparsec away from the galaxy
            if (!TryComp<PassiveDampeningComponent>(grid, out var dampening))
                dampening = AddComp<PassiveDampeningComponent>(grid);

            EntityManager.TryGetComponent(grid, out ShuttleComponent? shuttleComponent);

            // PassiveDampeningComponent conflicts with shuttles cruise control a frontier QOL and is resetting dampeners causing issues.
            // so if a station which shuttles have the station component too, then don't reset the physics to a near off state when it gets bumped
            dampening.Enabled = true;
            dampening.LinearDampening = shuttleComponent?.LinearDamping ?? 0.01f;
            dampening.AngularDampening = shuttleComponent?.AngularDamping ?? 0.01f;

        }
    }
}
