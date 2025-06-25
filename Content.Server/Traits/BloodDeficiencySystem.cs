// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Components;
using Content.Server.Body.Events;
using Content.Server.Traits.Assorted;
using Content.Shared.FixedPoint;

namespace Content.Server.Traits;

public sealed class BloodDeficiencySystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BloodDeficiencyComponent, NaturalBloodRegenerationAttemptEvent>(OnBloodRegen);
    }

    private void OnBloodRegen(Entity<BloodDeficiencyComponent> ent, ref NaturalBloodRegenerationAttemptEvent args)
    {
        if (!ent.Comp.Active || !TryComp<BloodstreamComponent>(ent.Owner, out var bloodstream))
            return;

        args.Amount = FixedPoint2.Min(args.Amount, 0) // If the blood regen amount already was negative, we keep it.
                      - bloodstream.BloodMaxVolume * ent.Comp.BloodLossPercentage;
    }
}
