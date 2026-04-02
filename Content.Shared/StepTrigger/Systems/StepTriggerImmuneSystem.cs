// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 Falcon
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Reflection.Metadata.Ecma335;
using Content.Shared.Examine;
using Content.Shared.Inventory;
using Content.Shared.StepTrigger.Components;

namespace Content.Shared.StepTrigger.Systems;

public sealed class StepTriggerImmuneSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<PreventableStepTriggerComponent, StepTriggerAttemptEvent>(OnStepTriggerClothingAttempt);
        SubscribeLocalEvent<PreventableStepTriggerComponent, ExaminedEvent>(OnExamined);
    }

    private void OnStepTriggerClothingAttempt(Entity<PreventableStepTriggerComponent> ent, ref StepTriggerAttemptEvent args)
    {
        // DEN: If the comp has a whitelist, it has been checked already.
        if (TryComp<ProtectedFromStepTriggersComponent>(args.Tripper, out var selfComp) && selfComp.Whitelist is null)
        {
            args.Cancelled = true;
        }
        else if (_inventory.TryGetInventoryEntity<ProtectedFromStepTriggersComponent>(args.Tripper, out _))
        {
            args.Cancelled = true;
        }
    }

    private void OnExamined(EntityUid uid, PreventableStepTriggerComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("clothing-required-step-trigger-examine"));
    }
}
