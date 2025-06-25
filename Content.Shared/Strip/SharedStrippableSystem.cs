// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 WarMechanic <69510347+WarMechanic@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DragDrop;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Popups;
using Content.Shared.Strip.Components;

namespace Content.Shared.Strip;

public abstract class SharedStrippableSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly ThievingSystem _thieving = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StrippingComponent, CanDropTargetEvent>(OnCanDropOn);
        SubscribeLocalEvent<StrippableComponent, CanDropDraggedEvent>(OnCanDrop);
        SubscribeLocalEvent<StrippableComponent, DragDropDraggedEvent>(OnDragDrop);
    }

    public (TimeSpan Time, ThievingStealth Stealth) GetStripTimeModifiers(EntityUid user, EntityUid target, TimeSpan initialTime)
    {
        var userEv = new BeforeStripEvent(initialTime);
        RaiseLocalEvent(user, ref userEv);
        var ev = new BeforeGettingStrippedEvent(userEv.Time, userEv.Stealth);
        RaiseLocalEvent(target, ref ev);
        return (ev.Time, ev.Stealth);
    }

    private void OnDragDrop(EntityUid uid, StrippableComponent component, ref DragDropDraggedEvent args)
    {
        // If the user drags a strippable thing onto themselves.
        if (args.Handled || args.Target != args.User)
            return;

        StartOpeningStripper(args.User, (uid, component));
        args.Handled = true;
    }

    public virtual void StartOpeningStripper(EntityUid user, Entity<StrippableComponent> component, bool openInCombat = false)
    {

    }

    private void OnCanDropOn(EntityUid uid, StrippingComponent component, ref CanDropTargetEvent args)
    {
        args.Handled = true;
        args.CanDrop |= uid == args.User &&
                        HasComp<StrippableComponent>(args.Dragged) &&
                        HasComp<HandsComponent>(args.User) &&
                        HasComp<StrippingComponent>(args.User);
    }

    private void OnCanDrop(EntityUid uid, StrippableComponent component, ref CanDropDraggedEvent args)
    {
        args.CanDrop |= args.Target == args.User &&
                        HasComp<StrippingComponent>(args.User) &&
                        HasComp<HandsComponent>(args.User);

        if (args.CanDrop)
            args.Handled = true;
    }

    public void StripPopup(string messageId, ThievingStealth stealth, EntityUid target, EntityUid? user = null, EntityUid? item = null, string slot = "")
    {
        bool subtle = stealth == ThievingStealth.Subtle;
        PopupType? popupSize = _thieving.GetPopupTypeFromStealth(stealth);

        if (popupSize.HasValue) // We should always have a value if we're not hidden
            _popup.PopupEntity(
                Loc.GetString(
                    messageId,
                ("user", subtle ? Loc.GetString("thieving-component-user") : user ?? EntityUid.Invalid),
                ("item", subtle ? Loc.GetString("thieving-component-item") : item ?? EntityUid.Invalid),
                ("slot", slot)),
            target,
            target,
            popupSize.Value);
    }

    public bool IsStripHidden(SlotDefinition definition, EntityUid? viewer)
    {
        if (!definition.StripHidden)
            return false;

        if (viewer == null)
            return true;

        return !(HasComp<BypassInteractionChecksComponent>(viewer) || HasComp<ThievingComponent>(viewer));
    }
}
