// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Drugs;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Robust.Client.Graphics;
using Robust.Shared.Timing;

namespace Content.Client.Drugs;

public sealed class MnemolithHighlightSystem : EquipmentHudSystem<SeeingMnemolithHighlightComponent>
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private MnemolithHighlightOverlay _highlightOverlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SeeingMnemolithHighlightComponent, SwitchableOverlayToggledEvent>(OnToggle);

        _highlightOverlay = new MnemolithHighlightOverlay();
    }

    private void OnToggle(Entity<SeeingMnemolithHighlightComponent> ent, ref SwitchableOverlayToggledEvent args)
    {
        RefreshOverlay(args.User);
    }

    protected override void UpdateInternal(RefreshEquipmentHudEvent<SeeingMnemolithHighlightComponent> args)
    {
        base.UpdateInternal(args);

        SeeingMnemolithHighlightComponent? comp = null;
        foreach (var c in args.Components)
        {
            if (!c.IsActive && (c.PulseTime <= 0f || c.PulseAccumulator >= c.PulseTime))
                continue;

            comp ??= c;
        }

        UpdateHighlightOverlay(comp);
    }

    protected override void DeactivateInternal()
    {
        base.DeactivateInternal();
        UpdateHighlightOverlay(null);
    }

    private void UpdateHighlightOverlay(SeeingMnemolithHighlightComponent? comp)
    {
        _highlightOverlay.Comp = comp;

        switch (comp)
        {
            case not null when !_overlayMan.HasOverlay<MnemolithHighlightOverlay>():
                _overlayMan.AddOverlay(_highlightOverlay);
                break;
            case null:
                _overlayMan.RemoveOverlay(_highlightOverlay);
                break;
        }
    }
}