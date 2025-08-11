// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._DEN.Body.Systems;

namespace Content.Server.Vampiric;

public sealed partial class BloodSuckerSystem
{
    // This is for consent purposes, as some entities may have toxic or drugged blood.
    private void OnStartup(Entity<BloodSuckerComponent> ent, ref ComponentStartup args)
    {
        if (!HasComp<BloodExaminerComponent>(ent))
            ent.Comp.AddedBloodExaminer = EnsureComp<BloodExaminerComponent>(ent);
    }

    private void OnShutdown(Entity<BloodSuckerComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.AddedBloodExaminer != null && HasComp<BloodExaminerComponent>(ent))
            RemCompDeferred(ent, ent.Comp.AddedBloodExaminer);
    }
}
