// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 theashtronaut <112137107+theashtronaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Popups;
using Content.Shared.Atmos;
using Content.Shared.Construction.Components;
using Content.Shared.Destructible;
using Content.Shared.Popups;
using JetBrains.Annotations;
using Robust.Shared.Player;

namespace Content.Server.Atmos.Piping.EntitySystems
{
    [UsedImplicitly]
    public sealed class AtmosUnsafeUnanchorSystem : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _atmosphere = default!;
        [Dependency] private readonly PopupSystem _popup = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<AtmosUnsafeUnanchorComponent, BeforeUnanchoredEvent>(OnBeforeUnanchored);
            SubscribeLocalEvent<AtmosUnsafeUnanchorComponent, UnanchorAttemptEvent>(OnUnanchorAttempt);
            SubscribeLocalEvent<AtmosUnsafeUnanchorComponent, BreakageEventArgs>(OnBreak);
        }

        private void OnUnanchorAttempt(EntityUid uid, AtmosUnsafeUnanchorComponent component, UnanchorAttemptEvent args)
        {
            if (!component.Enabled || !EntityManager.TryGetComponent(uid, out NodeContainerComponent? nodes))
                return;

            if (_atmosphere.GetContainingMixture(uid, true) is not {} environment)
                return;

            foreach (var node in nodes.Nodes.Values)
            {
                if (node is not PipeNode pipe)
                    continue;

                if (pipe.Air.Pressure - environment.Pressure > 2 * Atmospherics.OneAtmosphere)
                {
                    args.Delay += 2f;
                    _popup.PopupEntity(Loc.GetString("comp-atmos-unsafe-unanchor-warning"), pipe.Owner,
                        args.User, PopupType.MediumCaution);
                    return; // Show the warning only once.
                }
            }
        }

        private void OnBeforeUnanchored(EntityUid uid, AtmosUnsafeUnanchorComponent component, BeforeUnanchoredEvent args)
        {
            if (component.Enabled)
                LeakGas(uid);
        }

        private void OnBreak(EntityUid uid, AtmosUnsafeUnanchorComponent component, BreakageEventArgs args)
        {
            LeakGas(uid);
            // Can't use DoActsBehavior["Destruction"] in the same trigger because that would prevent us
            // from leaking. So we make up for this by queueing deletion here.
            QueueDel(uid);
        }

        /// <summary>
        /// Leak gas from the uid's NodeContainer into the tile atmosphere.
        /// </summary>
        public void LeakGas(EntityUid uid)
        {
            if (!EntityManager.TryGetComponent(uid, out NodeContainerComponent? nodes))
                return;

            if (_atmosphere.GetContainingMixture(uid, true, true) is not {} environment)
                environment = GasMixture.SpaceGas;

            var lost = 0f;
            var timesLost = 0;

            foreach (var node in nodes.Nodes.Values)
            {
                if (node is not PipeNode pipe)
                    continue;

                var difference = pipe.Air.Pressure - environment.Pressure;
                lost += difference * environment.Volume / (environment.Temperature * Atmospherics.R);
                timesLost++;
            }

            var sharedLoss = lost / timesLost;
            var buffer = new GasMixture();

            foreach (var node in nodes.Nodes.Values)
            {
                if (node is not PipeNode pipe)
                    continue;

                _atmosphere.Merge(buffer, pipe.Air.Remove(sharedLoss));
            }

            _atmosphere.Merge(environment, buffer);
        }
    }
}
