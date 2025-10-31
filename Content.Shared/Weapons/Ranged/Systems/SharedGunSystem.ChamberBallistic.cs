// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Errant
// SPDX-FileCopyrightText: 2023 Scribbles0
// SPDX-FileCopyrightText: 2023 TaralGit
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Volodius
// SPDX-FileCopyrightText: 2023 and_a
// SPDX-FileCopyrightText: 2024 BramvanZijp
// SPDX-FileCopyrightText: 2024 Kara
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2024 sleepyyapril
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Shared.Weapons.Ranged.Systems;

public abstract partial class SharedGunSystem
{
    protected virtual void InitializeChamberBallistic()
    {
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, ComponentInit>(OnChamberBallisticInit);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, MapInitEvent>(OnChamberBallisticMapInit);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, ComponentStartup>(OnChamberStartup);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, TakeAmmoEvent>(OnChamberBallisticTakeAmmo);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, GetAmmoCountEvent>(OnChamberAmmoCount);

        /*
         * Open and close bolts are separate verbs.
         * Racking does both in one hit and has a different sound (to avoid RSI + sounds cooler).
         */

        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, GetVerbsEvent<ActivationVerb>>(OnChamberActivationVerb);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, GetVerbsEvent<InteractionVerb>>(OnChamberInteractionVerb);

        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, ActivateInWorldEvent>(OnChamberActivate);
        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, UseInHandEvent>(OnChamberUse);

        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, ExaminedEvent>(OnChamberBallisticExamine);

        SubscribeLocalEvent<ChamberBallisticAmmoProviderComponent, InteractUsingEvent>(OnBallisticInteractUsing);
    }

    private void OnChamberBallisticInit(EntityUid uid, ChamberBallisticAmmoProviderComponent component, ComponentInit args)
    {
        component.Container = Containers.EnsureContainer<Container>(uid, "ballistic-ammo");
        // TODO: This is called twice though we need to support loading appearance data (and we need to call it on MapInit
        // to ensure it's correct).
        UpdateBallisticAppearance(uid, component);
    }

    private void OnChamberBallisticMapInit(EntityUid uid, ChamberBallisticAmmoProviderComponent component, MapInitEvent args)
    {
        // TODO this should be part of the prototype, not set on map init.
        // Alternatively, just track spawned count, instead of unspawned count.
        if (component.Proto != null)
        {
            component.UnspawnedCount = Math.Max(0, component.Capacity - component.Container.ContainedEntities.Count);
            UpdateBallisticAppearance(uid, component);
            Dirty(uid, component);
        }
    }

    private void OnChamberStartup(EntityUid uid, ChamberBallisticAmmoProviderComponent component, ComponentStartup args)
    {
        // Appearance data doesn't get serialized and want to make sure this is correct on spawn (regardless of MapInit) so.
        if (component.BoltClosed != null)
        {
            Appearance.SetData(uid, AmmoVisuals.BoltClosed, component.BoltClosed.Value);
        }
    }

    /// <summary>
    /// Called when user "Activated In World" (E) with the gun as the target
    /// </summary>
    private void OnChamberActivate(EntityUid uid, ChamberBallisticAmmoProviderComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = true;
        ToggleBolt(uid, component, args.User);
    }

    /// <summary>
    /// Called when gun was "Activated In Hand" (Z)
    /// </summary>
    private void OnChamberUse(EntityUid uid, ChamberBallisticAmmoProviderComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;
        if (component.CanRack)
            UseChambered(uid, component, args.User);
        else
            ToggleBolt(uid, component, args.User);
    }

    /// <summary>
    /// Creates "Rack" verb on the gun
    /// </summary>
    private void OnChamberActivationVerb(EntityUid uid, ChamberBallisticAmmoProviderComponent component, GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || component.BoltClosed == null || !component.CanRack)
            return;

        args.Verbs.Add(new ActivationVerb()
        {
            Text = Loc.GetString("gun-chamber-rack"),
            Act = () =>
            {
                UseChambered(uid, component, args.User);
            }
        });
    }

    /// <summary>
    /// Opens then closes the bolt, or just closes it if currently open.
    /// </summary>
    private void UseChambered(EntityUid uid, ChamberBallisticAmmoProviderComponent component, EntityUid? user = null)
    {
        if (component.BoltClosed == false)
        {
            ToggleBolt(uid, component, user);
            return;
        }

        if (TryTakeChamberEntity(uid, out var chamberEnt))
        {
            if (_netManager.IsServer)
            {
                EjectCartridge(chamberEnt.Value);
            }
            else
            {
                // Similar to below just due to prediction.
                TransformSystem.DetachEntity(chamberEnt.Value, Transform(chamberEnt.Value));
            }
        }

        if (!CycleCartridge(uid, component, user))
        {
            UpdateAmmoCount(uid);
        }

        if (component.BoltClosed != false)
        {
            Audio.PlayPredicted(component.SoundRack, uid, user);
        }
    }

    /// <summary>
    /// Creates "Open/Close bolt" verb on the gun
    /// </summary>
    private void OnChamberInteractionVerb(EntityUid uid, ChamberBallisticAmmoProviderComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || component.BoltClosed == null)
            return;

        args.Verbs.Add(new InteractionVerb()
        {
            Text = component.BoltClosed.Value ? Loc.GetString("gun-chamber-bolt-open") : Loc.GetString("gun-chamber-bolt-close"),
            Act = () =>
            {
                // Just toggling might be more user friendly instead of trying to set to whatever they think?
                ToggleBolt(uid, component, args.User);
            }
        });
    }

    /// <summary>
    /// Updates the bolt to its new state
    /// </summary>
    public void SetBoltClosed(EntityUid uid, ChamberBallisticAmmoProviderComponent component, bool value, EntityUid? user = null, AppearanceComponent? appearance = null, ItemSlotsComponent? slots = null)
    {
        if (component.BoltClosed == null || value == component.BoltClosed)
            return;

        Resolve(uid, ref appearance, ref slots, false);
        Appearance.SetData(uid, AmmoVisuals.BoltClosed, value, appearance);

        if (value)
        {
            CycleCartridge(uid, component, user, appearance);

            if (user != null)
                PopupSystem.PopupClient(Loc.GetString("gun-chamber-bolt-closed"), uid, user.Value);

            if (slots != null)
            {
                _slots.SetLock(uid, ChamberSlot, true, slots);
            }

            Audio.PlayPredicted(component.BoltClosedSound, uid, user);
        }
        else
        {
            if (TryTakeChamberEntity(uid, out var chambered))
            {
                if (_netManager.IsServer)
                {
                    EjectCartridge(chambered.Value);
                }
                else
                {
                    // Prediction moment
                    // The problem is client will dump the cartridge on the ground and the new server state
                    // won't correspond due to randomness so looks weird
                    // but we also need to always take it from the chamber or else ammocount won't be correct.
                    TransformSystem.DetachParentToNull(chambered.Value, Transform(chambered.Value));
                }

                UpdateAmmoCount(uid);
            }

            if (user != null)
                PopupSystem.PopupClient(Loc.GetString("gun-chamber-bolt-opened"), uid, user.Value);

            if (slots != null)
            {
                _slots.SetLock(uid, ChamberSlot, false, slots);
            }

            Audio.PlayPredicted(component.BoltOpenedSound, uid, user);
        }

        component.BoltClosed = value;
        Dirty(uid, component);
    }

    /// <summary>
    /// Tries to take ammo from the magazine and insert into the chamber.
    /// </summary>
    private bool CycleCartridge(EntityUid uid, ChamberBallisticAmmoProviderComponent component, EntityUid? user = null, AppearanceComponent? appearance = null)
    {
        // Try to put a new round in if possible.
        var chambered = GetChamberEntity(uid);
        var result = false;

        // Similar to what takeammo does though that uses an optimised version where
        // multiple bullets may be fired in a single tick.
        if (chambered == null)
        {
            var relayedArgs = new TakeAmmoEvent(1,
                new List<(EntityUid? Entity, IShootable Shootable)>(),
                Transform(uid).Coordinates,
                user);
            RaiseLocalEvent(uid, relayedArgs);

            if (relayedArgs.Ammo.Count > 0)
            {
                var newChamberEnt = relayedArgs.Ammo[0].Entity;
                TryInsertChamberBallistic(uid, newChamberEnt!.Value);
                var ammoEv = new GetAmmoCountEvent();
                RaiseLocalEvent(uid, ref ammoEv);
                UpdateAmmoCount(uid);

                // Clientside reconciliation things
                if (_netManager.IsClient)
                {
                    foreach (var (ent, _) in relayedArgs.Ammo)
                    {
                        if (!IsClientSide(ent!.Value))
                            continue;

                        Del(ent.Value);
                    }
                }
            }
            else
            {
                UpdateAmmoCount(uid);
            }

            result = true;
        }

        return result;
    }

    /// <summary>
    /// Sets the bolt's positional value to the other state
    /// </summary>
    public void ToggleBolt(EntityUid uid, ChamberBallisticAmmoProviderComponent component, EntityUid? user = null)
    {
        if (component.BoltClosed == null)
            return;

        SetBoltClosed(uid, component, !component.BoltClosed.Value, user);
    }

    /// <summary>
    /// Called when the gun was Examined
    /// </summary>
    private void OnChamberBallisticExamine(EntityUid uid, ChamberBallisticAmmoProviderComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var (count, _) = GetChamberBallisticCountCapacity(uid, component);
        string boltState;

        using (args.PushGroup(nameof(ChamberBallisticAmmoProviderComponent)))
        {
            if (component.BoltClosed != null)
            {
                if (component.BoltClosed == true)
                    boltState = Loc.GetString("gun-chamber-bolt-open-state");
                else
                    boltState = Loc.GetString("gun-chamber-bolt-closed-state");
                args.PushMarkup(Loc.GetString("gun-chamber-bolt", ("bolt", boltState),
                    ("color", component.BoltClosed.Value ? Color.FromHex("#94e1f2") : Color.FromHex("#f29d94"))));
            }

            args.PushMarkup(Loc.GetString("gun-magazine-examine", ("color", AmmoExamineColor), ("count", count)));
        }
    }

    protected (int, int) GetChamberBallisticCountCapacity(EntityUid uid, ChamberBallisticAmmoProviderComponent component)
    {
        var count = GetChamberEntity(uid) != null ? 1 : 0;
        var magCount = GetBallisticShots(component);
        var magCapacity = component.Capacity;
        return (count + magCount, magCapacity);
    }

    private bool TryInsertChamberBallistic(EntityUid uid, EntityUid ammo)
    {
        return Containers.TryGetContainer(uid, ChamberSlot, out var container) &&
               container is ContainerSlot slot &&
               Containers.Insert(ammo, slot);
    }

    private void OnChamberAmmoCount(EntityUid uid, ChamberBallisticAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        OnBallisticAmmoCount(uid, component, ref args);
        args.Capacity += 1;
        var chambered = GetChamberEntity(uid);

        if (chambered != null)
        {
            args.Count += 1;
        }
    }

    private void OnChamberBallisticTakeAmmo(EntityUid uid, ChamberBallisticAmmoProviderComponent component, TakeAmmoEvent args)
    {
        if (component.BoltClosed == false)
        {
            args.Reason = Loc.GetString("gun-chamber-bolt-ammo");
            return;
        }

        // So chamber logic is kinda sussier than the others
        // Essentially we want to treat the chamber as a potentially free slot and then the mag as the remaining slots
        // i.e. if we shoot 3 times, then we use the chamber once (regardless if it's empty or not) and 2 from the mag
        // We move the n + 1 shot into the chamber as we essentially treat it like a stack.
        TryComp<AppearanceComponent>(uid, out var appearance);

        EntityUid? chamberEnt;

        // Normal behaviour for guns.
        if (component.AutoCycle)
        {
            if (TryTakeChamberEntity(uid, out chamberEnt))
            {
                args.Ammo.Add((chamberEnt.Value, EnsureShootable(chamberEnt.Value)));
            }
            // No ammo returned.
            else
            {
                return;
            }

            // We pass in Shots not Shots - 1 as we'll take the last entity and move it into the chamber.
            var relayedArgs = new TakeAmmoEvent(args.Shots, new List<(EntityUid? Entity, IShootable Shootable)>(), args.Coordinates, args.User);
            RaiseLocalEvent(uid, relayedArgs);

            // Put in the nth slot back into the chamber
            // Rest of the ammo gets shot
            if (relayedArgs.Ammo.Count > 0)
            {
                var newChamberEnt = relayedArgs.Ammo[^1].Entity;
                TryInsertChamberBallistic(uid, newChamberEnt!.Value);
            }

            // Anything above the chamber-refill amount gets fired.
            for (var i = 0; i < relayedArgs.Ammo.Count - 1; i++)
            {
                args.Ammo.Add(relayedArgs.Ammo[i]);
            }

            // If no more ammo then open bolt.
            if (relayedArgs.Ammo.Count == 0)
            {
                SetBoltClosed(uid, component, false, user: args.User, appearance: appearance);
            }

            var ammoEv = new GetAmmoCountEvent();
            RaiseLocalEvent(uid, ref ammoEv);
        }
        // If gun doesn't autocycle (e.g. bolt-action weapons) then we leave the chambered entity in there but still return it.
        else if (Containers.TryGetContainer(uid, ChamberSlot, out var container) &&
                 container is ContainerSlot { ContainedEntity: not null } slot)
        {
            // Shooting code won't eject it if it's still contained.
            chamberEnt = slot.ContainedEntity;
            args.Ammo.Add((chamberEnt.Value, EnsureShootable(chamberEnt.Value)));
        }
    }

    private void OnBallisticInteractUsing(EntityUid uid, ChamberBallisticAmmoProviderComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (_whitelistSystem.IsWhitelistFailOrNull(component.Whitelist, args.Used))
            return;

        if (GetBallisticShots(component) >= component.Capacity)
            return;

        if (component.BoltClosed == true && component.ReloadWhenBolted == false)
            return;

        component.Entities.Add(args.Used);
        Containers.Insert(args.Used, component.Container);
        // Not predicted so
        Audio.PlayPredicted(component.SoundInsert, uid, args.User);
        args.Handled = true;
        UpdateBallisticAppearance(uid, component);
        Dirty(uid, component);
    }
}
