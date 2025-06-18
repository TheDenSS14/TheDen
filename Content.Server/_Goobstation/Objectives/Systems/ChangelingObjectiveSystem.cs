// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.Systems;

public sealed partial class ChangelingObjectiveSystem : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _number = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AbsorbConditionComponent, ObjectiveGetProgressEvent>(OnAbsorbGetProgress);
        SubscribeLocalEvent<StealDNAConditionComponent, ObjectiveGetProgressEvent>(OnStealDNAGetProgress);
    }

    private void OnAbsorbGetProgress(EntityUid uid, AbsorbConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        var target = _number.GetTarget(uid);
        if (target != 0)
            args.Progress = MathF.Min(comp.Absorbed / target, 1f);
        else args.Progress = 1f;
    }
    private void OnStealDNAGetProgress(EntityUid uid, StealDNAConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        var target = _number.GetTarget(uid);
        if (target != 0)
            args.Progress = MathF.Min(comp.DNAStolen / target, 1f);
        else args.Progress = 1f;
    }
}
