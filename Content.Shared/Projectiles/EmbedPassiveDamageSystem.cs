// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.CombatMode.Pacification;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Robust.Shared.Timing;

namespace Content.Shared.Projectiles;

public sealed class EmbedPassiveDamageSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly HashSet<EntityUid> _activeEmbeds = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EmbedPassiveDamageComponent, DamageOtherOnHitStartupEvent>(OnDamageOtherOnHitStartup);
        SubscribeLocalEvent<ItemToggleEmbedPassiveDamageComponent, ItemToggleDamageOtherOnHitStartupEvent>(OnItemToggleStartup);
        SubscribeLocalEvent<EmbedPassiveDamageComponent, EmbedEvent>(OnEmbed);
        SubscribeLocalEvent<EmbedPassiveDamageComponent, RemoveEmbedEvent>(OnRemoveEmbed);
        SubscribeLocalEvent<EmbedPassiveDamageComponent, ItemToggledEvent>(OnItemToggle);
        SubscribeLocalEvent<EmbedPassiveDamageComponent, AttemptPacifiedThrowEvent>(OnAttemptPacifiedThrow);
    }

    /// <summary>
    ///   Inherit stats from DamageOtherOnHit.
    /// </summary>
    private void OnDamageOtherOnHitStartup(EntityUid uid, EmbedPassiveDamageComponent component, DamageOtherOnHitStartupEvent args)
    {
        if (component.Damage.Empty)
            component.Damage = args.Weapon.Comp.Damage * component.ThrowingDamageMultiplier;
    }

    /// <summary>
    ///   Inherit stats from ItemToggleDamageOtherOnHit.
    /// </summary>
    private void OnItemToggleStartup(EntityUid uid, ItemToggleEmbedPassiveDamageComponent component, ItemToggleDamageOtherOnHitStartupEvent args)
    {
        if (!TryComp<EmbedPassiveDamageComponent>(uid, out var embedPassiveDamage) ||
            component.ActivatedDamage != null ||
            !(args.Weapon.Comp.ActivatedDamage is {} activatedDamage))
            return;

        component.ActivatedDamage = activatedDamage * embedPassiveDamage.ThrowingDamageMultiplier;
    }

    private void OnEmbed(EntityUid uid, EmbedPassiveDamageComponent component, EmbedEvent args)
    {
        if (component.Damage.Empty || component.Damage.GetTotal() == 0 ||
            !TryComp<MobStateComponent>(args.Embedded, out var mobState) ||
            !TryComp<DamageableComponent>(args.Embedded, out var damageable))
            return;

        component.Embedded = args.Embedded;
        component.EmbeddedDamageable = damageable;
        component.EmbeddedMobState = mobState;
        component.EmbeddedBodyPart = args.BodyPart;
        component.NextDamage = _timing.CurTime + TimeSpan.FromSeconds(1f);

        _activeEmbeds.Add(uid);
        Dirty(uid, component);
    }

    private void OnRemoveEmbed(EntityUid uid, EmbedPassiveDamageComponent component, RemoveEmbedEvent args)
    {
        component.Embedded = null;
        component.EmbeddedDamageable = null;
        component.EmbeddedMobState = null;
        component.EmbeddedBodyPart = null;
        component.NextDamage = TimeSpan.Zero;

        _activeEmbeds.Remove(uid);
        Dirty(uid, component);
    }

    /// <summary>
    ///   Used to update the EmbedPassiveDamageComponent component on item toggle.
    /// </summary>
    private void OnItemToggle(EntityUid uid, EmbedPassiveDamageComponent component, ItemToggledEvent args)
    {
        if (!TryComp<ItemToggleEmbedPassiveDamageComponent>(uid, out var itemTogglePassiveDamage))
            return;

        if (args.Activated && itemTogglePassiveDamage.ActivatedDamage is {} activatedDamage)
        {
            itemTogglePassiveDamage.DeactivatedDamage ??= component.Damage;
            component.Damage = activatedDamage;
        }
        else if (itemTogglePassiveDamage.DeactivatedDamage is {} deactivatedDamage)
            component.Damage = deactivatedDamage;
    }

    /// <summary>
    /// Prevent Pacified entities from throwing items that deal passive damage when embedded.
    /// </summary>
    private void OnAttemptPacifiedThrow(EntityUid uid, EmbedPassiveDamageComponent comp, ref AttemptPacifiedThrowEvent args)
    {
        // Allow healing projectiles, forbid any that do damage
        if (comp.Damage.AnyPositive())
            args.Cancel("pacified-cannot-throw");
    }

    public override void Update(float frameTime)
    {
        var curTime = _timing.CurTime;
        
        foreach (var ent in _activeEmbeds)
        {
            if (!Exists(ent) || !TryComp<EmbedPassiveDamageComponent>(ent, out var comp))
            {
                _activeEmbeds.Remove(ent);
                continue;
            }

            if (comp.Embedded is null || !Exists(comp.Embedded)
                || comp.EmbeddedDamageable is null
                || comp.EmbeddedMobState is null
                || comp.EmbeddedMobState.CurrentState == MobState.Dead)
            {
                _activeEmbeds.Remove(ent);
                continue;
            }

            if (comp.NextDamage > curTime)
                continue;

            comp.NextDamage = curTime + TimeSpan.FromSeconds(1f);
            _damageable.TryChangeDamage(comp.Embedded, comp.Damage, false, false, comp.EmbeddedDamageable, targetPart: comp.EmbeddedBodyPart);
        }
    }
}
