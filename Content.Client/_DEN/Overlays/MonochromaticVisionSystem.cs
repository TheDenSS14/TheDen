// SPDX-FileCopyrightText: 2026 Dirius
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Traits.Components;
using Content.Shared.CCVar;
using Robust.Client.Graphics;
using Robust.Shared.Configuration;
using Robust.Shared.Player;


namespace Content.Client._DEN.Overlays;


public sealed partial class MonochromaticVisionSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ISharedPlayerManager _playerManager = default!;

    private MonochromaticVisionOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<MonochromaticVisionComponent, ComponentInit>(OnMonochromaticVisionInit);
        SubscribeLocalEvent<MonochromaticVisionComponent, ComponentShutdown>(OnMonochromaticVisionShutdown);
        SubscribeLocalEvent<MonochromaticVisionComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<MonochromaticVisionComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);
        
        Subs.CVar(_cfg, CCVars.NoVisionFilters, OnNoVisionFiltersChanged);

        _overlay = new();
    }

    private void OnMonochromaticVisionInit(Entity<MonochromaticVisionComponent> entity, ref ComponentInit args)
    {
        if (entity != _playerManager.LocalEntity)
            return;

        if (!_cfg.GetCVar(CCVars.NoVisionFilters))
            _overlayManager.AddOverlay(_overlay);
    }
    
    private void OnMonochromaticVisionShutdown(Entity<MonochromaticVisionComponent> entity, ref ComponentShutdown args)
    {
        if (entity != _playerManager.LocalEntity)
            return;
        
        _overlayManager.RemoveOverlay(_overlay);
    }

    private void OnPlayerAttached(Entity<MonochromaticVisionComponent> entity, ref LocalPlayerAttachedEvent evt)
    {
        if (!_cfg.GetCVar(CCVars.NoVisionFilters))
            _overlayManager.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(Entity<MonochromaticVisionComponent> entity, ref LocalPlayerDetachedEvent evt)
    {
        _overlayManager.RemoveOverlay(_overlay);
    }

    private void OnNoVisionFiltersChanged(bool enabled)
    {
        if (enabled)
            _overlayManager.RemoveOverlay(_overlay);
        else
            _overlayManager.AddOverlay(_overlay);
    }
}
