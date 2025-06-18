// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Shared.Psionics.Glimmer;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.ReactionEffects;

[DataDefinition]
public sealed partial class ChangeGlimmerReactionEffect : EntityEffect
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-change-glimmer-reaction-effect", ("chance", Probability),
            ("count", Count));

    /// <summary>
    ///     Added to glimmer when reaction occurs.
    /// </summary>
    [DataField]
    public float Count = 1;

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs _)
            return;

        var glimmerSystem = args.EntityManager.EntitySysManager.GetEntitySystem<GlimmerSystem>();

        glimmerSystem.DeltaGlimmerInput(Count);
    }
}
