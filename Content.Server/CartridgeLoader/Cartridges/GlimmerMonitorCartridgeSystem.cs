// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Server.Psionics.Glimmer;

namespace Content.Server.CartridgeLoader.Cartridges;

public sealed class GlimmerMonitorCartridgeSystem : EntitySystem
{
    [Dependency] private readonly CartridgeLoaderSystem? _cartridgeLoaderSystem = default!;
    [Dependency] private readonly PassiveGlimmerReductionSystem _glimmerReductionSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GlimmerMonitorCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
        SubscribeLocalEvent<GlimmerMonitorCartridgeComponent, CartridgeMessageEvent>(OnMessage);
    }

    /// <summary>
    /// This gets called when the ui fragment needs to be updated for the first time after activating
    /// </summary>
    private void OnUiReady(EntityUid uid, GlimmerMonitorCartridgeComponent component, CartridgeUiReadyEvent args)
    {
        UpdateUiState(uid, args.Loader, component);
    }

    private void OnMessage(EntityUid uid, GlimmerMonitorCartridgeComponent component, CartridgeMessageEvent args)
    {
        if (args is not GlimmerMonitorSyncMessageEvent)
            return;

        UpdateUiState(uid, EntityManager.GetEntity(args.LoaderUid), component);
    }

    public void UpdateUiState(EntityUid uid, EntityUid loaderUid, GlimmerMonitorCartridgeComponent? component)
    {
        if (!Resolve(uid, ref component))
            return;

        var state = new GlimmerMonitorUiState(_glimmerReductionSystem.GlimmerValues);
        _cartridgeLoaderSystem?.UpdateCartridgeUiState(loaderUid, state);
    }
}
