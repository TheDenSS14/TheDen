// SPDX-FileCopyrightText: 2025 AirFryerBuyOneGetOneFree
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 deltanedas
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Kitchen.Components;
using Robust.Shared.Containers;

namespace Content.Server._Goobstation.Kitchen;

/// <summary>
/// Prevents automation taking items out of an active microwave.
/// Only exists because microwave supercode only prevents it in interaction, not attempt events.
/// </summary>
public sealed class MicrowaveEventsSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ActiveMicrowaveComponent, ContainerIsRemovingAttemptEvent>(OnRemoveAttempt);
    }

    private void OnRemoveAttempt(Entity<ActiveMicrowaveComponent> ent, ref ContainerIsRemovingAttemptEvent args)
    {
        if (ent.Comp.CookTimeRemaining > 0)
            args.Cancel();
    }
}
