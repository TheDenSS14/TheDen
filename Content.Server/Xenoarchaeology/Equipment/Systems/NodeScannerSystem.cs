// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 AJCM-git <60196617+ajcm-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Popups;
using Content.Server.Xenoarchaeology.Equipment.Components;
using Content.Server.Xenoarchaeology.XenoArtifacts;
using Content.Shared.Interaction;
using Content.Shared.Timing;

namespace Content.Server.Xenoarchaeology.Equipment.Systems;

public sealed class NodeScannerSystem : EntitySystem
{
    [Dependency] private readonly UseDelaySystem _useDelay = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<NodeScannerComponent, AfterInteractEvent>(OnAfterInteract);
    }

    private void OnAfterInteract(EntityUid uid, NodeScannerComponent component, AfterInteractEvent args)
    {
        if (!args.CanReach || args.Target == null)
            return;

        if (!TryComp<ArtifactComponent>(args.Target, out var artifact) || artifact.CurrentNodeId == null)
            return;

        if (args.Handled)
            return;
        args.Handled = true;

        var target = args.Target.Value;

        if (TryComp(uid, out UseDelayComponent? useDelay)
            && !_useDelay.TryResetDelay((uid, useDelay), true))
            return;

        _popupSystem.PopupEntity(Loc.GetString("node-scan-popup",
            ("id", $"{artifact.CurrentNodeId}")), target);
    }
}
