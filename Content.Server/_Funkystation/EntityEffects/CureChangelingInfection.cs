// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Content.Shared.Changeling;

namespace Content.Server.EntityEffects.Effects;

/// <summary>
/// Do I need to elaborate on what this does?
/// </summary>
[UsedImplicitly]
public sealed partial class CureChangelingInfection : EntityEffect
{
    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) =>
        Loc.GetString("reagent-effect-guidebook-cure-changeling", ("chance", Probability));

    public override void Effect(EntityEffectBaseArgs args) =>
        args.EntityManager.RemoveComponent<ChangelingInfectionComponent>(args.TargetEntity);
}
