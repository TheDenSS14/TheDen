// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using Content.Shared.EntityEffects;
using Content.Shared.Jittering;
using Content.Shared.WhiteDream.BloodCult.BloodCultist;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.ReagentEffects;

[UsedImplicitly]
public sealed partial class PurifyEvil : EntityEffect
{
    [DataField]
    public float Amplitude = 10.0f;

    [DataField]
    public float Frequency = 4.0f;

    [DataField]
    public TimeSpan Time = TimeSpan.FromSeconds(30.0f);

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("reagent-effect-guidebook-purify-evil");
    }

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs e)
            return;

        var entityManager = args.EntityManager;
        var uid = args.TargetEntity;
        if (!entityManager.TryGetComponent(uid, out BloodCultistComponent? bloodCultist) ||
            bloodCultist.DeconvertToken is not null)
            return;

        entityManager.System<SharedJitteringSystem>().DoJitter(uid, Time, true, Amplitude, Frequency);

        bloodCultist.DeconvertToken = new CancellationTokenSource();
        Robust.Shared.Timing.Timer.Spawn(Time, () => DeconvertCultist(uid, entityManager),
            bloodCultist.DeconvertToken.Token);
    }

    private void DeconvertCultist(EntityUid uid, IEntityManager entityManager)
    {
        if (entityManager.HasComponent<BloodCultistComponent>(uid))
            entityManager.RemoveComponent<BloodCultistComponent>(uid);
    }
}
