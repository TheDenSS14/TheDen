// SPDX-FileCopyrightText: 2022 CommieFlowers
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 rolfero
// SPDX-FileCopyrightText: 2023 Chief-Engineer
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 Doru991
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Errant
// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 PixelTK
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 Vordenburg
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 Dakamakat
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 OldDanceJacket
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 gluesniffler
// SPDX-FileCopyrightText: 2024 sleepyyapril
// SPDX-FileCopyrightText: 2024 username
// SPDX-FileCopyrightText: 2025 Aviu00
// SPDX-FileCopyrightText: 2025 Eagle-0
// SPDX-FileCopyrightText: 2025 Eris
// SPDX-FileCopyrightText: 2025 Princess Cheeseballs
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Linq;
using Content.Shared._Goobstation.MartialArts;
using Content.Shared._Goobstation.MartialArts.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Alert;
using Content.Shared.CombatMode;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Database;
using Content.Shared.Effects;
using Content.Shared.IdentityManagement;
using Content.Shared.Jittering;
using Content.Shared.Popups;
using Content.Shared.Projectiles;
using Content.Shared.Rejuvenate;
using Content.Shared.Rounding;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee.Events;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Serialization;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Content.Shared.FixedPoint;

namespace Content.Shared.Damage.Systems;

public abstract partial class SharedStaminaSystem : EntitySystem
{
    [Dependency] protected readonly IGameTiming _timing = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly MetaDataSystem _metadata = default!;
    [Dependency] private readonly SharedColorFlashEffectSystem _color = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] protected readonly SharedStunSystem _stunSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffect = default!; // goob edit
    [Dependency] private readonly SharedStutteringSystem _stutter = default!; // goob edit
    [Dependency] private readonly SharedJitteringSystem _jitter = default!; // goob edit
    [Dependency] private readonly ClothingModifyStunTimeSystem _modify = default!; // goob edit

    /// <summary>
    /// How much of a buffer is there between the stun duration and when stuns can be re-applied.
    /// </summary>
    protected static readonly TimeSpan StamCritBufferTime = TimeSpan.FromSeconds(3f);

    public float UniversalStaminaDamageModifier { get; private set; } = 1f;

    public override void Initialize()
    {
        base.Initialize();

        InitializeModifier();

        SubscribeLocalEvent<StaminaComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<StaminaComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<StaminaComponent, AfterAutoHandleStateEvent>(OnStamHandleState);
        SubscribeLocalEvent<StaminaComponent, DisarmedEvent>(OnDisarmed);
        SubscribeLocalEvent<StaminaComponent, RejuvenateEvent>(OnRejuvenate);

        SubscribeLocalEvent<StaminaDamageOnEmbedComponent, EmbedEvent>(OnProjectileEmbed);

        SubscribeLocalEvent<StaminaDamageOnCollideComponent, ProjectileHitEvent>(OnProjectileHit);
        SubscribeLocalEvent<StaminaDamageOnCollideComponent, ThrowDoHitEvent>(OnThrowHit);

        SubscribeLocalEvent<StaminaDamageOnHitComponent, MeleeHitEvent>(OnMeleeHit);
    }

    protected virtual void OnStamHandleState(Entity<StaminaComponent> entity, ref AfterAutoHandleStateEvent args)
    {
        if (entity.Comp.Critical)
            EnterStamCrit(entity, null, true);
        else
        {
            if (entity.Comp.StaminaDamage > 0f)
                EnsureComp<ActiveStaminaComponent>(entity);

            ExitStamCrit(entity);
        }
    }

    protected virtual void OnShutdown(Entity<StaminaComponent> entity, ref ComponentShutdown args)
    {
        if (MetaData(entity).EntityLifeStage < EntityLifeStage.Terminating)
        {
            RemCompDeferred<ActiveStaminaComponent>(entity);
        }

        SetStaminaAlert(entity);
    }

    private void OnStartup(Entity<StaminaComponent> entity, ref ComponentStartup args)
    {
        UpdateStaminaVisuals(entity);
    }

    [PublicAPI]
    public float GetStaminaDamage(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return 0f;

        var curTime = _timing.CurTime;
        var pauseTime = _metadata.GetPauseTime(uid);
        return MathF.Max(0f, component.StaminaDamage - MathF.Max(0f, (float) (curTime - (component.NextUpdate + pauseTime)).TotalSeconds * component.Decay));
    }

    private void OnRejuvenate(Entity<StaminaComponent> entity, ref RejuvenateEvent args)
    {
        if (entity.Comp.StaminaDamage >= entity.Comp.CritThreshold)
        {
            ExitStamCrit(entity, entity.Comp);
        }

        entity.Comp.StaminaDamage = 0;
        AdjustSlowdown(entity.Owner);
        RemComp<ActiveStaminaComponent>(entity);
        UpdateStaminaVisuals(entity);
        Dirty(entity);
    }

    private void OnDisarmed(EntityUid uid, StaminaComponent component, DisarmedEvent args)
    {
        if (args.Handled || !_random.Prob(args.PushProbability))
            return;

        if (component.Critical)
            return;

        var damage = args.PushProbability * component.CritThreshold;
        TakeStaminaDamage(uid, damage, component, source: args.Source);

        // We need a better method of getting if the entity is going to resist stam damage, both this and the lines in the foreach at the end of OnHit() are awful
        if (!component.Critical)
            return;

        var targetEnt = Identity.Entity(args.Target, EntityManager);
        var sourceEnt = Identity.Entity(args.Source, EntityManager);

        _popup.PopupEntity(Loc.GetString("stunned-component-disarm-success-others", ("source", sourceEnt), ("target", targetEnt)), targetEnt, Filter.PvsExcept(args.Source), true, PopupType.LargeCaution);
        _popup.PopupCursor(Loc.GetString("stunned-component-disarm-success", ("target", targetEnt)), args.Source, PopupType.Large);

        _adminLogger.Add(LogType.DisarmedKnockdown, LogImpact.Medium, $"{ToPrettyString(args.Source):user} knocked down {ToPrettyString(args.Target):target}");

        args.Handled = true;
    }

    private void OnMeleeHit(EntityUid uid, StaminaDamageOnHitComponent component, MeleeHitEvent args)
    {
        if (!args.IsHit ||
            !args.HitEntities.Any() ||
            component.Damage <= 0f)
        {
            return;
        }

        // Goobstation - Martial Arts
        if (TryComp<MartialArtsKnowledgeComponent>(args.User, out var knowledgeComp)
            && TryComp<MartialArtBlockedComponent>(args.Weapon, out var blockedComp)
            && knowledgeComp.MartialArtsForm == blockedComp.Form)
            return;
        // Goobstation

        var ev = new StaminaDamageOnHitAttemptEvent();
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        var stamQuery = GetEntityQuery<StaminaComponent>();
        var toHit = new List<(EntityUid Entity, StaminaComponent Component)>();

        // Split stamina damage between all eligible targets.
        foreach (var ent in args.HitEntities)
        {
            if (!stamQuery.TryGetComponent(ent, out var stam))
                continue;

            toHit.Add((ent, stam));
        }

        foreach (var (ent, comp) in toHit)
        {
            var hitEvent = new TakeStaminaDamageEvent(ent);
            RaiseLocalEvent(uid, hitEvent);

            if (hitEvent.Handled)
                return;

            var damageImmediate = component.Damage;
            damageImmediate *= hitEvent.Multiplier;
            damageImmediate += hitEvent.FlatModifier;

            damageImmediate *= hitEvent.Multiplier;

            damageImmediate += hitEvent.FlatModifier;

            TakeStaminaDamage(ent, damageImmediate / toHit.Count, comp, source: args.User, with: args.Weapon, sound: component.Sound, immediate: true);
            TakeOvertimeStaminaDamage(ent, component.Overtime, comp);
        }
    }

    private void OnProjectileHit(EntityUid uid, StaminaDamageOnCollideComponent component, ref ProjectileHitEvent args)
    {
        OnCollide(uid, component, args.Target);
    }

    private void OnProjectileEmbed(EntityUid uid, StaminaDamageOnEmbedComponent component, ref EmbedEvent args)
    {
        if (!TryComp<StaminaComponent>(args.Embedded, out var stamina))
            return;

        TakeStaminaDamage(args.Embedded, component.Damage, stamina, source: uid);
        TakeOvertimeStaminaDamage(uid, component.Overtime, stamina);
    }

    private void OnThrowHit(EntityUid uid, StaminaDamageOnCollideComponent component, ThrowDoHitEvent args)
    {
        OnCollide(uid, component, args.Target);
    }

    private void OnCollide(EntityUid uid, StaminaDamageOnCollideComponent component, EntityUid target)
    {
        if (!TryComp<StaminaComponent>(target, out var stamComp))
            return;

        var ev = new StaminaDamageOnHitAttemptEvent();
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        TakeStaminaDamage(target, component.Damage, source: uid, sound: component.Sound);
        TakeOvertimeStaminaDamage(uid, component.Overtime, stamComp);
    }

    private void UpdateStaminaVisuals(Entity<StaminaComponent> entity)
    {
        SetStaminaAlert(entity, entity.Comp);
        SetStaminaAnimation(entity);
    }

    // Here so server can properly tell all clients in PVS range to start the animation
    protected virtual void SetStaminaAnimation(Entity<StaminaComponent> entity){}

    private void SetStaminaAlert(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Deleted)
        {
            _alerts.ClearAlert(uid, "Stamina");
            return;
        }

        var severity = ContentHelpers.RoundToLevels(MathF.Max(0f, component.CritThreshold - component.StaminaDamage), component.CritThreshold, 7);
        _alerts.ShowAlert(uid, component.StaminaAlert, (short) severity);
    }

    /// <summary>
    /// Tries to take stamina damage without raising the entity over the crit threshold.
    /// </summary>
    public bool TryTakeStamina(EntityUid uid, float value, StaminaComponent? component = null, EntityUid? source = null, EntityUid? with = null)
    {
        // Something that has no Stamina component automatically passes stamina checks
        if (!Resolve(uid, ref component, false))
            return true;

        var oldStam = component.StaminaDamage;

        if (oldStam + value > component.CritThreshold || component.Critical)
            return false;

        TakeStaminaDamage(uid, value, component, source, with, visual: false);
        return true;
    }

    // goob edit - stunmeta
    public void TakeOvertimeStaminaDamage(EntityUid uid, float value, StaminaComponent component)
    {
        // do this only on server side because otherwise shit happens (Coderabbit do not bitch at me about the profanity I swear to God)
        if (value == 0)
            return;

        var hasComp = TryComp<OvertimeStaminaDamageComponent>(uid, out var overtime);

        if (!hasComp)
            overtime = EnsureComp<OvertimeStaminaDamageComponent>(uid);

        overtime!.Amount = hasComp ? overtime.Amount + value : value;
        overtime!.Damage = hasComp ? overtime.Damage + value : value;
    }

    // goob edit - stunmeta
    public void TakeStaminaDamage(EntityUid uid, float value, StaminaComponent? component = null,
        EntityUid? source = null, EntityUid? with = null, bool visual = true, SoundSpecifier? sound = null, bool? allowsSlowdown = true, bool immediate = true)
    {
        if (!Resolve(uid, ref component, false)
            || value == 0)
            return;

        var ev = new BeforeStaminaDamageEvent(value);
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        // Have we already reached the point of max stamina damage?
        if (component.Critical && immediate)
        {
            EnterStamCrit(uid, component, true); // enter stamcrit
            return;
        }

        var oldDamage = component.StaminaDamage;
        component.StaminaDamage = MathF.Max(0f, component.StaminaDamage + value);

        // Reset the decay cooldown upon taking damage.
        if (oldDamage < component.StaminaDamage)
        {
            var nextUpdate = _timing.CurTime + TimeSpan.FromSeconds(component.Cooldown);

            if (component.NextUpdate < nextUpdate)
                component.NextUpdate = nextUpdate;
        }

        if (allowsSlowdown == true)
            AdjustSlowdown(uid);

        UpdateStaminaVisuals((uid, component));

        // Checking if the stamina damage has decreased to zero after exiting the stamcrit
        if (component.AfterCritical && oldDamage > component.StaminaDamage && component.StaminaDamage <= 0f)
        {
            // goob edit - stunmeta
            // no slowdown because funny
            _jitter.DoJitter(uid, TimeSpan.FromSeconds(10f), true);
            _stutter.DoStutter(uid, TimeSpan.FromSeconds(10f), true);
        }

        UpdateStaminaVisuals((uid, component));

        if (!component.Critical && component.StaminaDamage >= component.CritThreshold && value > 0)
            EnterStamCrit(uid, component, immediate);
        else if (component.StaminaDamage < component.CritThreshold)
            ExitStamCrit(uid, component);

        EnsureComp<ActiveStaminaComponent>(uid);
        Dirty(uid, component);

        if (value <= 0)
            return;
        if (source != null)
        {
            _adminLogger.Add(LogType.Stamina, $"{ToPrettyString(source.Value):user} caused {value} stamina damage to {ToPrettyString(uid):target}{(with != null ? $" using {ToPrettyString(with.Value):using}" : "")}");
        }
        else
        {
            _adminLogger.Add(LogType.Stamina, $"{ToPrettyString(uid):target} took {value} stamina damage");
        }

        if (visual)
        {
            _color.RaiseEffect(Color.Aqua, new List<EntityUid>() { uid }, Filter.Pvs(uid, entityManager: EntityManager));
        }

        if (_net.IsServer)
        {
            _audio.PlayPvs(sound, uid);
        }
    }

    public void ToggleStaminaDrain(EntityUid target, float drainRate, bool enabled, bool modifiesSpeed, EntityUid? source = null)
    {
        if (!TryComp<StaminaComponent>(target, out var stamina))
            return;

        // If theres no source, we assume its the target that caused the drain.
        var actualSource = source ?? target;

        if (enabled)
        {
            stamina.ActiveDrains[actualSource] = (drainRate, modifiesSpeed);
            EnsureComp<ActiveStaminaComponent>(target);
        }
        else
            stamina.ActiveDrains.Remove(actualSource);

        Dirty(target, stamina);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        if (!_timing.IsFirstTimePredicted)
            return;

        var stamQuery = GetEntityQuery<StaminaComponent>();
        var query = EntityQueryEnumerator<ActiveStaminaComponent>();
        var curTime = _timing.CurTime;
        while (query.MoveNext(out var uid, out _))
        {
            // Just in case we have active but not stamina we'll check and account for it.
            if (!stamQuery.TryGetComponent(uid, out var comp) ||
                comp.StaminaDamage <= 0f && !comp.Critical && comp.ActiveDrains.Count == 0)
            {
                RemComp<ActiveStaminaComponent>(uid);
                continue;
            }
            if (comp.ActiveDrains.Count > 0)
                foreach (var (source, (drainRate, modifiesSpeed)) in comp.ActiveDrains)
                    TakeStaminaDamage(uid,
                    drainRate * frameTime,
                    comp,
                    source: source,
                    visual: false,
                    allowsSlowdown: modifiesSpeed);
            // Shouldn't need to consider paused time as we're only iterating non-paused stamina components.
            var nextUpdate = comp.NextUpdate;

            if (nextUpdate > curTime)
                continue;

            // We were in crit so come out of it and continue.
            if (comp.Critical)
            {
                ExitStamCrit(uid, comp);
                continue;
            }

            comp.NextUpdate += TimeSpan.FromSeconds(1f);
            // If theres no active drains, recover stamina.
            if (comp.ActiveDrains.Count == 0)
                TakeStaminaDamage(uid, -comp.Decay, comp);

            Dirty(uid, comp);
        }
    }

    // goob edit - stunmeta
    private void EnterStamCrit(EntityUid uid, StaminaComponent? component = null, bool hardStun = false)
    {
        if (!Resolve(uid, ref component) || !hardStun && component.Critical)
        {
            return;
        }

        // if our entity is under stims make threshold bigger
        if (TryComp<StamcritResistComponent>(uid, out var stamres)
        && component.StaminaDamage < component.CritThreshold * stamres.Multiplier)
            return;

        if (!hardStun)
        {
            if (!_statusEffect.HasStatusEffect(uid, "KnockedDown"))
                _stunSystem.TryKnockdown(uid, component.StunTime, true);
            return;
        }

        // you got batonned hard.
        component.Critical = true;
        component.StaminaDamage = component.CritThreshold;

        if (_stunSystem.TryParalyze(uid, component.StunTime, true))
            _stunSystem.TrySeeingStars(uid);
        _stunSystem.TryParalyze(uid, component.StunTime, true);

        component.NextUpdate = _timing.CurTime + component.StunTime * _modify.GetModifier(uid) + StamCritBufferTime;

        EnsureComp<ActiveStaminaComponent>(uid);
        Dirty(uid, component);

        _adminLogger.Add(LogType.Stamina, LogImpact.Medium, $"{ToPrettyString(uid):user} entered stamina crit");
    }

    private void ExitStamCrit(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component) ||
            !component.Critical)
        {
            return;
        }

        component.Critical = false;
        component.StaminaDamage = 0f;
        component.NextUpdate = _timing.CurTime;
        SetStaminaAlert(uid, component);
        AdjustSlowdown(uid);
        RemComp<ActiveStaminaComponent>(uid);
        Dirty(uid, component);
        _adminLogger.Add(LogType.Stamina, LogImpact.Low, $"{ToPrettyString(uid):user} recovered from stamina crit");
    }


    /// <summary>
    /// Adjusts the movement speed of an entity based on its current <see cref="StaminaComponent.StaminaDamage"/> value.
    /// If the entity has a <see cref="SlowOnDamageComponent"/>, its custom damage-to-speed thresholds are used,
    /// otherwise, a default set of thresholds is applied.
    /// The method determines the closest applicable damage threshold below the crit limit and applies the corresponding
    /// speed modifier using the stun system. If no threshold is met then the entity's speed is restored to normal.
    /// </summary>
    /// <param name="ent">Entity to update</param>
    private void AdjustSlowdown(Entity<StaminaComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        var closest = FixedPoint2.Zero;

        // Iterate through the dictionary in the similar way as in Damage.SlowOnDamageSystem.OnRefreshMovespeed
        foreach (var thres in ent.Comp.StunModifierThresholds)
        {
            var key = thres.Key.Float();

            if (ent.Comp.StaminaDamage >= key && key > closest && closest < ent.Comp.CritThreshold)
                closest = thres.Key;
        }

        _stunSystem.UpdateStunModifiers(ent, ent.Comp.StunModifierThresholds[closest]);
    }
}

/// <summary>
///     Raised before stamina damage is dealt to allow other systems to cancel it.
/// </summary>
[ByRefEvent]
public record struct BeforeStaminaDamageEvent(float Value, bool Cancelled = false);

[Serializable, NetSerializable]
public sealed class StaminaAnimationEvent(NetEntity entity) : EntityEventArgs
{
    public NetEntity Entity = entity;
}
