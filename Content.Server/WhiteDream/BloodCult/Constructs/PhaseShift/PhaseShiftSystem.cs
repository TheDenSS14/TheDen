// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Eye;
using Content.Shared.WhiteDream.BloodCult.Constructs.PhaseShift;
using Robust.Server.GameObjects;

namespace Content.Server.WhiteDream.BloodCult.Constructs.PhaseShift;

public sealed class PhaseShiftSystem : SharedPhaseShiftSystem
{
    [Dependency] private readonly VisibilitySystem _visibilitySystem = default!;

    protected override void OnComponentStartup(Entity<PhaseShiftedComponent> ent, ref ComponentStartup args)
    {
        base.OnComponentStartup(ent, ref args);

        if (!TryComp<VisibilityComponent>(ent, out var visibility))
            return;

        _visibilitySystem.AddLayer((ent, visibility), (int) VisibilityFlags.Ghost, false);
        _visibilitySystem.RemoveLayer((ent, visibility), (int) VisibilityFlags.Normal, false);
        _visibilitySystem.RefreshVisibility(ent);
    }

    protected override void OnComponentShutdown(Entity<PhaseShiftedComponent> ent, ref ComponentShutdown args)
    {
        base.OnComponentShutdown(ent, ref args);

        if (!TryComp<VisibilityComponent>(ent, out var visibility))
            return;

        _visibilitySystem.RemoveLayer((ent, visibility), (int) VisibilityFlags.Ghost, false);
        _visibilitySystem.AddLayer((ent, visibility), (int) VisibilityFlags.Normal, false);
        _visibilitySystem.RefreshVisibility(ent);
    }
}
