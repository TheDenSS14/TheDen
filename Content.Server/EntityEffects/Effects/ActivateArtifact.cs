// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Xenoarchaeology.XenoArtifacts;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Content.Shared.EntityEffects;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class ActivateArtifact : EntityEffect
{
    public override void Effect(EntityEffectBaseArgs args)
    {
        var artifact = args.EntityManager.EntitySysManager.GetEntitySystem<ArtifactSystem>();
        artifact.TryActivateArtifact(args.TargetEntity);
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) =>
        Loc.GetString("reagent-effect-guidebook-activate-artifact", ("chance", Probability));
}
