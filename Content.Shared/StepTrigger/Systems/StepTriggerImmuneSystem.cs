// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Examine;
using Content.Shared.Inventory;
using Content.Shared.StepTrigger.Components;
using Content.Shared.Tag;

namespace Content.Shared.StepTrigger.Systems;

public sealed class StepTriggerImmuneSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<StepTriggerImmuneComponent, StepTriggerAttemptEvent>(OnStepTriggerAttempt);
        SubscribeLocalEvent<ClothingRequiredStepTriggerComponent, StepTriggerAttemptEvent>(OnStepTriggerClothingAttempt);
        SubscribeLocalEvent<ClothingRequiredStepTriggerComponent, ExaminedEvent>(OnExamined);
    }

    private void OnStepTriggerAttempt(Entity<StepTriggerImmuneComponent> ent, ref StepTriggerAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void OnStepTriggerClothingAttempt(EntityUid uid, ClothingRequiredStepTriggerComponent component, ref StepTriggerAttemptEvent args)
    {
        if (_inventory.TryGetInventoryEntity<ClothingRequiredStepTriggerImmuneComponent>(args.Tripper, out _))
        {
            args.Cancelled = true;
        }
    }

    private void OnExamined(EntityUid uid, ClothingRequiredStepTriggerComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("clothing-required-step-trigger-examine"));
    }
}
