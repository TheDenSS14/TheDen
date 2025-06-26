// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Emisse <99158783+Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 IntegerTempest <30300017+IntegerTempest@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 MisterMecky <mrmecky@hotmail.com>
// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 Blu <79374236+BlueHNT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 PrPleGoo <prplegoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2024 beck-thompson <107373427+beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chat;
using Content.Shared._Goobstation.Flashbang;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Climbing.Events;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Electrocution;
using Content.Shared.Explosion;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Gravity;
using Content.Shared.IdentityManagement.Components;
using Content.Shared.Inventory.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Overlays;
using Content.Shared.Radio;
using Content.Shared.Slippery;
using Content.Shared.Strip.Components;
using Content.Shared.Temperature;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Chat;
using Content.Shared.Overlays.Switchable;

using Content.Shared.Stunnable;
using Content.Shared.Movement.Events;
using Content.Shared._DEN.Body;

namespace Content.Shared.Inventory;

public partial class InventorySystem
{
    public void InitializeRelay()
    {
        SubscribeLocalEvent<InventoryComponent, DamageModifyEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, TakeStaminaDamageEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, ElectrocutionAttemptEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SlipAttemptEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshMovementSpeedModifiersEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, BeforeStripEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SeeIdentityAttemptEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, ModifyChangedTemperatureEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, GetDefaultRadioChannelEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshNameModifiersEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, GetFlashbangedEvent>(RelayInventoryEvent); // goob edit
        SubscribeLocalEvent<InventoryComponent, TransformSpeakerNameEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SelfBeforeHyposprayInjectsEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, TargetBeforeHyposprayInjectsEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SelfBeforeGunShotEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SelfBeforeClimbEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, MakeFootstepSoundEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, CannotSupportStandingEvent>(RelayInventoryEvent);

        // by-ref events
        SubscribeLocalEvent<InventoryComponent, GetExplosionResistanceEvent>(RefRelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, IsWeightlessEvent>(RefRelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, ModifySlowOnDamageSpeedEvent>(RefRelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, ModifyStunTimeEvent>(RefRelayInventoryEvent); // goob edit

        // Eye/vision events
        SubscribeLocalEvent<InventoryComponent, CanSeeAttemptEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, GetEyeProtectionEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, GetBlurEvent>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, SolutionScanEvent>(RelayInventoryEvent);

        // ComponentActivatedClientSystems
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowJobIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowHealthBarsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowHealthIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowHungerIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowThirstIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowMindShieldIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowSyndicateIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowCriminalRecordIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ShowPsionicsRecordIconsComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<NightVisionComponent>>(RelayInventoryEvent);
        SubscribeLocalEvent<InventoryComponent, RefreshEquipmentHudEvent<ThermalVisionComponent>>(RelayInventoryEvent);

        SubscribeLocalEvent<InventoryComponent, GetVerbsEvent<EquipmentVerb>>(OnGetEquipmentVerbs);
    }

    protected void RefRelayInventoryEvent<T>(EntityUid uid, InventoryComponent component, ref T args) where T : IInventoryRelayEvent
    {
        RelayEvent((uid, component), ref args);
    }

    protected void RelayInventoryEvent<T>(EntityUid uid, InventoryComponent component, T args) where T : IInventoryRelayEvent
    {
        RelayEvent((uid, component), args);
    }

    public void RelayEvent<T>(Entity<InventoryComponent> inventory, ref T args) where T : IInventoryRelayEvent
    {
        if (args.TargetSlots == SlotFlags.NONE)
            return;

        // this copies the by-ref event if it is a struct
        var ev = new InventoryRelayedEvent<T>(args);
        var enumerator = new InventorySlotEnumerator(inventory, args.TargetSlots);
        while (enumerator.NextItem(out var item))
        {
            RaiseLocalEvent(item, ev);
        }

        // and now we copy it back
        args = ev.Args;
    }

    public void RelayEvent<T>(Entity<InventoryComponent> inventory, T args) where T : IInventoryRelayEvent
    {
        if (args.TargetSlots == SlotFlags.NONE)
            return;

        var ev = new InventoryRelayedEvent<T>(args);
        var enumerator = new InventorySlotEnumerator(inventory, args.TargetSlots);
        while (enumerator.NextItem(out var item))
        {
            RaiseLocalEvent(item, ev);
        }
    }

    private void OnGetEquipmentVerbs(EntityUid uid, InventoryComponent component, GetVerbsEvent<EquipmentVerb> args)
    {
        // Automatically relay stripping related verbs to all equipped clothing.
        var ev = new InventoryRelayedEvent<GetVerbsEvent<EquipmentVerb>>(args);
        var enumerator = new InventorySlotEnumerator(component);
        while (enumerator.NextItem(out var item, out var slotDef))
        {
            if (!_strippable.IsStripHidden(slotDef, args.User) || args.User == uid)
                RaiseLocalEvent(item, ev);
        }
    }

}

/// <summary>
///     Event wrapper for relayed events.
/// </summary>
/// <remarks>
///      This avoids nested inventory relays, and makes it easy to have certain events only handled by the initial
///      target entity. E.g. health based movement speed modifiers should not be handled by a hat, even if that hat
///      happens to be a dead mouse. Clothing that wishes to modify movement speed must subscribe to
///      InventoryRelayedEvent&lt;RefreshMovementSpeedModifiersEvent&gt;
/// </remarks>
public sealed class InventoryRelayedEvent<TEvent> : EntityEventArgs
{
    public TEvent Args;

    public InventoryRelayedEvent(TEvent args)
    {
        Args = args;
    }
}

public interface IClothingSlots
{
    SlotFlags Slots { get; }
}

/// <summary>
///     Events that should be relayed to inventory slots should implement this interface.
/// </summary>
public interface IInventoryRelayEvent
{
    /// <summary>
    ///     What inventory slots should this event be relayed to, if any?
    /// </summary>
    /// <remarks>
    ///     In general you may want to exclude <see cref="SlotFlags.POCKET"/>, given that those items are not truly
    ///     "equipped" by the user.
    /// </remarks>
    public SlotFlags TargetSlots { get; }
}
