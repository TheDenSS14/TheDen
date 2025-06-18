// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Mobs.Components;
using Content.Shared.Timing;
using Content.Server.Explosion.EntitySystems; // Why is trigger under explosions by the way? Even doors already use it.
using Content.Server.Electrocution;
using Robust.Shared.Containers;

namespace Content.Server.ShockCollar;

public sealed partial class ShockCollarSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ElectrocutionSystem _electrocutionSystem = default!;
    [Dependency] private readonly UseDelaySystem _useDelay = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ShockCollarComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(EntityUid uid, ShockCollarComponent component, TriggerEvent args)
    {
        if (!_container.TryGetContainingContainer(uid, out var container)) // Try to get the entity directly containing this
            return;

        var containerEnt = container.Owner;

        if (!HasComp<MobStateComponent>(containerEnt)) // If it's not a mob we don't care
            return;

        // DeltaV: prevent clocks from instantly killing people
        if (TryComp<UseDelayComponent>(uid, out var useDelay)
            && !_useDelay.TryResetDelay((uid, useDelay), true))
            return;

        _electrocutionSystem.TryDoElectrocution(containerEnt, null, component.ShockDamage, component.ShockTime, true, ignoreInsulation: true);
    }
}

