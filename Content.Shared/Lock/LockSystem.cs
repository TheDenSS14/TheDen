// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Ygg01
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 MilenVolf
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 Timemaster99
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2024 exincore
// SPDX-FileCopyrightText: 2024 nikthechampiongr
// SPDX-FileCopyrightText: 2024 sleepyyapril
// SPDX-FileCopyrightText: 2025 Winter
// SPDX-FileCopyrightText: 2025 creku
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.ActionBlocker; // DEN for locking
using Content.Shared.Construction.Components;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Systems;
using Content.Shared.Examine;
using Content.Shared.Hands.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Content.Shared.UserInterface;
using Content.Shared.Verbs;
using Content.Shared.Wires;
using Content.Shared.Item.ItemToggle.Components;
using JetBrains.Annotations;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Utility;

namespace Content.Shared.Lock;

/// <summary>
/// DEN - the states of toggling a lock
/// </summary>
public enum LockToggleResult
{
    /// <summary>
    /// can't lock for whatever reason
    /// </summary>
    Failure,

    /// <summary>
    /// can lock
    /// </summary>
    Success,

    /// <summary>
    /// can lock, but you're doing it from inside
    /// </summary>
    SuccessFromInside
}

/// <summary>
/// Handles (un)locking and examining of Lock components
/// </summary>
[UsedImplicitly]
public sealed class LockSystem : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly ActivatableUISystem _activatableUI = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _sharedPopupSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;

    /// <inheritdoc />
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LockComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<LockComponent, ActivateInWorldEvent>(OnActivated);
        SubscribeLocalEvent<LockComponent, StorageOpenAttemptEvent>(OnStorageOpenAttempt);
        SubscribeLocalEvent<LockComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<LockComponent, GetVerbsEvent<AlternativeVerb>>(AddToggleLockVerb);
        SubscribeLocalEvent<LockComponent, GotEmaggedEvent>(OnEmagged);
        SubscribeLocalEvent<LockComponent, LockDoAfter>(OnDoAfterLock);
        SubscribeLocalEvent<LockComponent, UnlockDoAfter>(OnDoAfterUnlock);
        SubscribeLocalEvent<LockComponent, StorageInteractAttemptEvent>(OnStorageInteractAttempt);

        SubscribeLocalEvent<LockedWiresPanelComponent, LockToggleAttemptEvent>(OnLockToggleAttempt);
        SubscribeLocalEvent<LockedWiresPanelComponent, AttemptChangePanelEvent>(OnAttemptChangePanel);
        SubscribeLocalEvent<LockedAnchorableComponent, UnanchorAttemptEvent>(OnUnanchorAttempt);

        SubscribeLocalEvent<ActivatableUIRequiresLockComponent, ActivatableUIOpenAttemptEvent>(OnUIOpenAttempt);
        SubscribeLocalEvent<ActivatableUIRequiresLockComponent, LockToggledEvent>(LockToggled);

        SubscribeLocalEvent<ItemToggleRequiresLockComponent, ItemToggleActivateAttemptEvent>(OnActivateAttempt);
    }

    private void OnStartup(EntityUid uid, LockComponent lockComp, ComponentStartup args)
    {
        _appearanceSystem.SetData(uid, LockVisuals.Locked, lockComp.Locked);
    }

    private void OnActivated(EntityUid uid, LockComponent lockComp, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        // Only attempt an unlock by default on Activate
        if (lockComp.Locked && lockComp.UnlockOnClick)
        {
            if (!lockComp.UnlockOnClick)
                return;
            TryUnlock(uid, args.User, lockComp);
            args.Handled = true;
        }
        else if (!lockComp.Locked && lockComp.LockOnClick)
        {
            TryLock(uid, args.User, lockComp);
            args.Handled = true;
        }
    }

    private void OnStorageOpenAttempt(EntityUid uid, LockComponent component, ref StorageOpenAttemptEvent args)
    {
        if (!component.Locked)
            return;

        if (!args.Silent)
            _sharedPopupSystem.PopupClient(Loc.GetString("entity-storage-component-locked-message"), uid, args.User);

        args.Cancelled = true;
    }

    private void OnExamined(EntityUid uid, LockComponent lockComp, ExaminedEvent args)
    {
        args.PushText(Loc.GetString(lockComp.Locked
                ? "lock-comp-on-examined-is-locked"
                : "lock-comp-on-examined-is-unlocked",
            ("entityName", Identity.Name(uid, EntityManager))));
    }

    /// <summary>
    /// Attmempts to lock a given entity
    /// </summary>
    /// <remarks>
    /// If the lock is set to require a do-after, a true return value only indicates that the do-after started.
    /// </remarks>
    /// <param name="uid">The entity with the lock</param>
    /// <param name="user">The person trying to lock it</param>
    /// <param name="lockComp"></param>
    /// <param name="skipDoAfter">If true, skip the required do-after if one is configured.</param>
    /// <returns>If locking was successful</returns>
    public bool TryLock(EntityUid uid, EntityUid user, LockComponent? lockComp = null, bool skipDoAfter = false)
    {
        if (!Resolve(uid, ref lockComp))
            return false;

        // DEN EDIT - CanToggleLock has three outcomes, so it's now an enum
        var canToggleLock = CanToggleLock(uid, user, quiet: false);
        if (canToggleLock == LockToggleResult.Failure)
            return false;

        if (lockComp.UseAccess && !HasUserAccess(uid, user, quiet: false))
            return false;

        // DEN ADDITION - lockTime can now either be the component's lock time, OR the FromInside time.
        // given that lockTime is only used by deployable barrier, it's unlikely this will conflict with anything
        var lockTime = lockComp.LockTime;
        if (canToggleLock == LockToggleResult.SuccessFromInside) {
            lockTime = lockComp.InsideToggleTime;
        }
        // END DEN ADD

        if (!skipDoAfter && lockTime != TimeSpan.Zero)
        {
            if (canToggleLock == LockToggleResult.SuccessFromInside) { // DEN ADDITION
                _audio.PlayPredicted(lockComp.InsideToggleSound, uid, user);
                _sharedPopupSystem.PopupClient(Loc.GetString("inside-lock-toggle-attempt"), uid, user);
            }

            return _doAfter.TryStartDoAfter(
                new DoAfterArgs(EntityManager, user, lockTime, new LockDoAfter(), uid, uid)
                {
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    RequireCanInteract = true,
                    NeedHand = true
                });
        }

        Lock(uid, user, lockComp);
        return true;
    }

    /// <summary>
    ///     Forces a given entity to be locked, does not activate a do-after.
    /// </summary>
    public void Lock(EntityUid uid, EntityUid? user, LockComponent? lockComp = null)
    {
        if (!Resolve(uid, ref lockComp))
            return;

        if (lockComp.Locked)
            return;

        if (user is { Valid: true })
        {
            _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-do-lock-success",
                ("entityName", Identity.Name(uid, EntityManager))), uid, user);
            _audio.PlayPredicted(lockComp.LockSound, uid, user);
        }

        lockComp.Locked = true;
        _appearanceSystem.SetData(uid, LockVisuals.Locked, true);
        Dirty(uid, lockComp);

        var ev = new LockToggledEvent(true);
        RaiseLocalEvent(uid, ref ev, true);
    }

    /// <summary>
    /// Forces a given entity to be unlocked
    /// </summary>
    /// <remarks>
    /// This does not process do-after times.
    /// </remarks>
    /// <param name="uid">The entity with the lock</param>
    /// <param name="user">The person unlocking it. Can be null</param>
    /// <param name="lockComp"></param>
    public void Unlock(EntityUid uid, EntityUid? user, LockComponent? lockComp = null)
    {
        if (!Resolve(uid, ref lockComp))
            return;

        if (!lockComp.Locked)
            return;

        if (user is { Valid: true })
        {
            _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-do-unlock-success",
                ("entityName", Identity.Name(uid, EntityManager))), uid, user.Value);
        }

        _audio.PlayPredicted(lockComp.UnlockSound, uid, user);

        lockComp.Locked = false;
        _appearanceSystem.SetData(uid, LockVisuals.Locked, false);
        Dirty(uid, lockComp);

        var ev = new LockToggledEvent(false);
        RaiseLocalEvent(uid, ref ev, true);
    }


    /// <summary>
    /// Attmempts to unlock a given entity
    /// </summary>
    /// <remarks>
    /// If the lock is set to require a do-after, a true return value only indicates that the do-after started.
    /// </remarks>
    /// <param name="uid">The entity with the lock</param>
    /// <param name="user">The person trying to unlock it</param>
    /// <param name="lockComp"></param>
    /// <param name="skipDoAfter">If true, skip the required do-after if one is configured.</param>
    /// <returns>If locking was successful</returns>
    public bool TryUnlock(EntityUid uid, EntityUid user, LockComponent? lockComp = null, bool skipDoAfter = false)
    {
        if (!Resolve(uid, ref lockComp))
            return false;

        // DEN EDIT - CanToggleLock has three outcomes, so it's now an enum
        var canToggleLock = CanToggleLock(uid, user, quiet: false);
        if (canToggleLock == LockToggleResult.Failure)
            return false;

        if (lockComp.UseAccess && !HasUserAccess(uid, user, quiet: false))
            return false;

        // DEN ADDITION - lockTime can now either be the component's lock time, OR the FromInside time.
        // given that lockTime is only used by deployable barrier, it's unlikely this will conflict with anything
        var lockTime = lockComp.LockTime;
        if (canToggleLock == LockToggleResult.SuccessFromInside) {
            lockTime = lockComp.InsideToggleTime;
        }
        // END DEN ADD

        if (!skipDoAfter && lockTime != TimeSpan.Zero)
        {
            if (canToggleLock == LockToggleResult.SuccessFromInside) { // DEN ADDITION
                _audio.PlayPredicted(lockComp.InsideToggleSound, uid, user);
                _sharedPopupSystem.PopupClient(Loc.GetString("inside-lock-toggle-attempt"), uid, user);
            }

            return _doAfter.TryStartDoAfter(
                new DoAfterArgs(EntityManager, user, lockTime, new UnlockDoAfter(), uid, uid)
                {
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    RequireCanInteract = true,
                    NeedHand = true
                });
        }

        Unlock(uid, user, lockComp);
        return true;
    }

    /// <summary>
    /// Returns true if the entity is locked.
    /// Entities with no lock component are considered unlocked.
    /// </summary>
    public bool IsLocked(Entity<LockComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return false;

        return ent.Comp.Locked;
    }

    /// <summary>
    /// Raises an event for other components to check whether or not
    /// the entity can be locked in its current state.
    /// 
    /// DEN EDIT: CHANGED THIS TO RETURN A LockToggleResult ENUM
    /// in order to allow for triggering the 'FromInside' doAfter via component events
    /// </summary>
    public LockToggleResult CanToggleLock(EntityUid uid, EntityUid user, bool quiet = true)
    {
        if (!HasComp<HandsComponent>(user))
            return LockToggleResult.Failure;

        if (!_actionBlocker.CanComplexInteract(user))
            return LockToggleResult.Failure;

        var ev = new LockToggleAttemptEvent(user, quiet);
        RaiseLocalEvent(uid, ref ev, true);
        if (ev.Cancelled)
            return LockToggleResult.Failure;

        if (ev.FromInside)
            return LockToggleResult.SuccessFromInside;

        var userEv = new UserLockToggleAttemptEvent(uid, quiet);
        RaiseLocalEvent(user, ref userEv, true);

        if (userEv.Cancelled)
            return LockToggleResult.Failure;

        return LockToggleResult.Success;
    }

    private bool HasUserAccess(EntityUid uid, EntityUid user, AccessReaderComponent? reader = null, bool quiet = true)
    {
        // Not having an AccessComponent means you get free access. woo!
        if (!Resolve(uid, ref reader, false))
            return true;

        if (_accessReader.IsAllowed(user, uid, reader))
            return true;

        if (!quiet)
            _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-has-user-access-fail"), uid, user);
        return false;
    }

    private void AddToggleLockVerb(EntityUid uid, LockComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        AlternativeVerb verb = new()
        {
            Act = component.Locked
                ? () => TryUnlock(uid, args.User, component)
                : () => TryLock(uid, args.User, component),
            Text = Loc.GetString(component.Locked ? "toggle-lock-verb-unlock" : "toggle-lock-verb-lock"),
            Icon = !component.Locked
                ? new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/lock.svg.192dpi.png"))
                : new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/unlock.svg.192dpi.png")),
        };
        args.Verbs.Add(verb);
    }

    private void OnEmagged(EntityUid uid, LockComponent component, ref GotEmaggedEvent args)
    {
        if (!component.Locked || !component.BreakOnEmag)
            return;

        _audio.PlayPredicted(component.UnlockSound, uid, args.UserUid);

        component.Locked = false;
        _appearanceSystem.SetData(uid, LockVisuals.Locked, false);
        Dirty(uid, component);

        var ev = new LockToggledEvent(false);
        RaiseLocalEvent(uid, ref ev, true);

        RemComp<LockComponent>(uid); //Literally destroys the lock as a tell it was emagged
        args.Handled = true;
    }

    private void OnDoAfterLock(EntityUid uid, LockComponent component, LockDoAfter args)
    {
        if (args.Cancelled)
            return;

        TryLock(uid, args.User, skipDoAfter: true);
    }

    private void OnDoAfterUnlock(EntityUid uid, LockComponent component, UnlockDoAfter args)
    {
        if (args.Cancelled)
            return;

        TryUnlock(uid, args.User, skipDoAfter: true);
    }

    private void OnStorageInteractAttempt(Entity<LockComponent> ent, ref StorageInteractAttemptEvent args)
    {
        if (ent.Comp.Locked)
            args.Cancelled = true;
    }

    private void OnLockToggleAttempt(Entity<LockedWiresPanelComponent> ent, ref LockToggleAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (!TryComp<WiresPanelComponent>(ent, out var panel) || !panel.Open)
            return;

        if (!args.Silent)
        {
            _sharedPopupSystem.PopupClient(Loc.GetString("construction-step-condition-wire-panel-close"),
                ent,
                args.User);
        }
        args.Cancelled = true;
    }


    private void OnAttemptChangePanel(Entity<LockedWiresPanelComponent> ent, ref AttemptChangePanelEvent args)
    {
        if (args.Cancelled)
            return;

        if (!TryComp<LockComponent>(ent, out var lockComp) || !lockComp.Locked)
            return;

        _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-generic-fail",
            ("target", Identity.Entity(ent, EntityManager))),
            ent,
            args.User);
        args.Cancelled = true;
    }

    private void OnUnanchorAttempt(Entity<LockedAnchorableComponent> ent, ref UnanchorAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (!TryComp<LockComponent>(ent, out var lockComp) || !lockComp.Locked)
            return;

        _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-generic-fail",
                ("target", Identity.Entity(ent, EntityManager))),
            ent,
            args.User);
        args.Cancel();
    }

    private void OnUIOpenAttempt(EntityUid uid, ActivatableUIRequiresLockComponent component, ActivatableUIOpenAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (TryComp<LockComponent>(uid, out var lockComp) && lockComp.Locked != component.RequireLocked)
        {
            args.Cancel();
            if (lockComp.Locked)
                _sharedPopupSystem.PopupEntity(Loc.GetString("entity-storage-component-locked-message"), uid, args.User);
        }
    }

    private void LockToggled(EntityUid uid, ActivatableUIRequiresLockComponent component, LockToggledEvent args)
    {
        if (!TryComp<LockComponent>(uid, out var lockComp) || lockComp.Locked == component.RequireLocked)
            return;

        _activatableUI.CloseAll(uid);
    }
    private void OnActivateAttempt(EntityUid uid, ItemToggleRequiresLockComponent component, ref ItemToggleActivateAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (TryComp<LockComponent>(uid, out var lockComp) && lockComp.Locked != component.RequireLocked)
        {
            args.Cancelled = true;
            if (lockComp.Locked)
                _sharedPopupSystem.PopupClient(Loc.GetString("lock-comp-generic-fail",
                ("target", Identity.Entity(uid, EntityManager))),
                uid,
                args.User);
        }
    }
}
