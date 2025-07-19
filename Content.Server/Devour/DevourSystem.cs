// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 PilgrimViis
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2024 cynical
// SPDX-FileCopyrightText: 2024 zelezniciar1
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 Rouden
// SPDX-FileCopyrightText: 2025 Roudenn
// SPDX-FileCopyrightText: 2025 coderabbitai[bot]
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Damage;
using Content.Shared.Devour;
using Content.Shared.Devour.Components;
using Content.Shared.Humanoid;
using Content.Shared.Item; // Goobstation

namespace Content.Server.Devour;

public sealed class DevourSystem : SharedDevourSystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstreamSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DevourerComponent, DevourDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<DevourerComponent, BeingGibbedEvent>(OnGibContents);
    }

    private void OnDoAfter(EntityUid uid, DevourerComponent component, DevourDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled || !TryComp<DamageableComponent>(uid, out var damageable))
            return;

        // Don't bother trying to heal/inject ichor if target isn't humanoid
        if (component.FoodPreference == FoodPreference.All ||
            (component.FoodPreference == FoodPreference.Humanoid && HasComp<HumanoidAppearanceComponent>(args.Args.Target)))
        {
            // Immediately heal damage
            _damageable.TryChangeDamage(uid, component.HealDamage, true, false, damageable);

            // Regeneration for a few seconds after
            var ichorInjection = new Solution(component.Chemical, component.HealRate);

            _bloodstreamSystem.TryAddToChemicals(uid, ichorInjection);

            if (component.ShouldStoreDevoured && args.Args.Target is not null && args.AllowDevouring)
                ContainerSystem.Insert(args.Args.Target.Value, component.Stomach);
        }
        // Goobstation start - Item devouring
        else if (args.Args.Target is { } target && HasComp<ItemComponent>(target))
        {
            if (component.ShouldStoreDevoured)
                ContainerSystem.Insert(target, component.Stomach);
            else
                QueueDel(target);
        }
        // Goobstation end
        //TODO: Figure out a better way of removing structures via devour that still entails standing still and waiting for a DoAfter. Somehow.
        //If it's not human, it must be a structure
        else if (args.Args.Target != null)
            QueueDel(args.Args.Target.Value);

        if (args.AllowDevouring)
            _audioSystem.PlayPvs(component.SoundDevour, uid);
    }

    private void OnGibContents(EntityUid uid, DevourerComponent component, ref BeingGibbedEvent args)
    {
        if (!component.ShouldStoreDevoured)
            return;

        // For some reason we have two different systems that should handle gibbing,
        // and for some another reason GibbingSystem, which should empty all containers, doesn't get involved in this process
        ContainerSystem.EmptyContainer(component.Stomach);
    }
}

