// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 coolmankid12345
// SPDX-FileCopyrightText: 2024 SimpleStation14
// SPDX-FileCopyrightText: 2024 Wrexbe (Josh)
// SPDX-FileCopyrightText: 2024 nikthechampiongr
// SPDX-FileCopyrightText: 2024 sleepyyapril
// SPDX-FileCopyrightText: 2025 Tayrtahn
// SPDX-FileCopyrightText: 2025 Terkala
// SPDX-FileCopyrightText: 2025 Toastermeister
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Linq;
using Content.Shared.Ghost;
using Content.Shared.Humanoid;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Content.Shared.Zombies;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.Prototypes;

namespace Content.Client.Zombies;

public sealed class ZombieSystem : SharedZombieSystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ZombieComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ZombieComponent, GetStatusIconsEvent>(GetZombieIcon);
        SubscribeLocalEvent<InitialInfectedComponent, GetStatusIconsEvent>(GetInitialInfectedIcon);
        SubscribeLocalEvent<ZombieTumorInfectionComponent, GetStatusIconsEvent>(GetInfectionStageIcon);
    }

    private void GetZombieIcon(Entity<ZombieComponent> ent, ref GetStatusIconsEvent args)
    {
        var iconPrototype = _prototype.Index(ent.Comp.StatusIcon);
        args.StatusIcons.Add(iconPrototype);
    }

    private void GetInitialInfectedIcon(Entity<InitialInfectedComponent> ent, ref GetStatusIconsEvent args)
    {
        if (HasComp<ZombieComponent>(ent))
            return;

        var iconPrototype = _prototype.Index(ent.Comp.StatusIcon);
        args.StatusIcons.Add(iconPrototype);
    }

    private void GetInfectionStageIcon(Entity<ZombieTumorInfectionComponent> ent, ref GetStatusIconsEvent args)
    {
        // Skip if already a full zombie (they should use the zombie icon instead)
        if (HasComp<ZombieComponent>(ent))
            return;

        // Don't show infection stage icons to the player themselves (only show to ghosts/admins)
        // Stage 5 (full zombie) will still show via GetZombieIcon
        var viewer = _playerManager.LocalSession?.AttachedEntity;
        if (viewer == ent.Owner)
            return;

        // Map infection stage to icon prototype ID
        var iconId = ent.Comp.Stage switch
        {
            ZombieTumorInfectionStage.Incubation => "ZombieInfectionIncubation",
            ZombieTumorInfectionStage.Early => "ZombieInfectionEarly",
            ZombieTumorInfectionStage.TumorFormed => "ZombieInfectionTumorFormed",
            ZombieTumorInfectionStage.Advanced => "ZombieInfectionAdvanced",
            _ => null
        };

        if (iconId != null)
        {
            var iconPrototype = _prototype.Index<FactionIconPrototype>(iconId);
            args.StatusIcons.Add(iconPrototype);
        }
    }

    private void OnStartup(EntityUid uid, ZombieComponent component, ComponentStartup args)
    {
        if (HasComp<HumanoidAppearanceComponent>(uid))
            return;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        for (var i = 0; i < sprite.AllLayers.Count(); i++)
        {
            sprite.LayerSetColor(i, component.SkinColor);
        }
    }
}
