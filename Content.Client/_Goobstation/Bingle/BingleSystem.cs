// SPDX-FileCopyrightText: 2025 Your Name <EctoplasmIsGood@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.GameObjects;
using Content.Shared._Goobstation.Bingle;
using Content.Shared.CombatMode;

namespace Content.Client._Goobstation.Bingle;

/// <summary>
///   Handles the appearance of bingles.
/// </summary>
public sealed class BingleSystem : EntitySystem
{
    [Dependency] private readonly AppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BingleComponent, ToggleCombatActionEvent>(OnCombatToggle);
        SubscribeLocalEvent<BingleComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    /// <summary>
    /// Makes the eyes glow red when combat mode is engaged.
    /// </summary>
    private void OnCombatToggle(EntityUid uid, BingleComponent component, ToggleCombatActionEvent args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;
        _appearance.OnChangeData(uid, sprite);
    }

    public void OnAppearanceChange(EntityUid uid, BingleComponent component, ref AppearanceChangeEvent args)
    {
        var sprite = args.Sprite;
        if (sprite == null)
            return;
        if (!TryComp<CombatModeComponent>(uid, out var combat))
            return;
        if (!sprite.LayerMapTryGet(BingleVisual.Combat, out var layer))
            return;
        sprite.LayerSetVisible(layer, combat.IsInCombatMode);
    }
}
