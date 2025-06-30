// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Abilities.Psionics;
using Content.Shared.Actions.Events;

namespace Content.Server.Abilities.Psionics;

public sealed partial class AnomalyPowerSystem
{
    /// <summary>
    ///     This function handles emulating the effects of a "Explosion Anomaly", using the caster as the "Anomaly",
    ///     while substituting their Psionic casting stats for "Severity and Stability".
    ///     Generates an explosion centered on the caster.
    /// </summary>
    private void DoExplosionAnomalyEffects(EntityUid uid, PsionicComponent component, AnomalyPowerActionEvent args, bool overcharged = false)
    {
        if (args.Explosion is null)
            return;

        if (overcharged)
            ExplosionSupercrit(uid, component, args);
        else ExplosionPulse(uid, component, args);
    }

    private void ExplosionSupercrit(EntityUid uid, PsionicComponent component, AnomalyPowerActionEvent args)
    {
        if (args.Explosion!.Value.SupercritExplosionPrototype is null)
            return;

        var explosion = args.Explosion!.Value;
        _boom.QueueExplosion(
            uid,
            explosion.SupercritExplosionPrototype,
            explosion.SupercritTotalIntensity * component.CurrentAmplification,
            explosion.SupercritDropoff / component.CurrentDampening,
            explosion.SupercritMaxTileIntensity * component.CurrentDampening
        );
    }

    private void ExplosionPulse(EntityUid uid, PsionicComponent component, AnomalyPowerActionEvent args)
    {
        if (args.Explosion!.Value.ExplosionPrototype is null)
            return;

        var explosion = args.Explosion!.Value;
        _boom.QueueExplosion(
            uid,
            explosion.ExplosionPrototype,
            explosion.TotalIntensity * component.CurrentAmplification,
            explosion.Dropoff / component.CurrentDampening,
            explosion.MaxTileIntensity * component.CurrentDampening
        );
    }
}