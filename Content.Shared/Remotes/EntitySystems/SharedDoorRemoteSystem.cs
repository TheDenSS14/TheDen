// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Popups;
using Content.Shared.Interaction.Events;
using Content.Shared.Remotes.Components;

namespace Content.Shared.Remotes.EntitySystems;

public abstract class SharedDoorRemoteSystem : EntitySystem
{
    [Dependency] protected readonly SharedPopupSystem Popup = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<DoorRemoteComponent, UseInHandEvent>(OnInHandActivation);
    }

    private void OnInHandActivation(Entity<DoorRemoteComponent> entity, ref UseInHandEvent args)
    {
        string switchMessageId;
        switch (entity.Comp.Mode)
        {
            case OperatingMode.OpenClose:
                entity.Comp.Mode = OperatingMode.ToggleBolts;
                switchMessageId = "door-remote-switch-state-toggle-bolts";
                break;

            // Skip toggle bolts mode and move on from there (to emergency access)
            case OperatingMode.ToggleBolts:
                entity.Comp.Mode = OperatingMode.ToggleEmergencyAccess;
                switchMessageId = "door-remote-switch-state-toggle-emergency-access";
                break;

            // Skip ToggleEmergencyAccess mode and move on from there (to door toggle)
            case OperatingMode.ToggleEmergencyAccess:
                entity.Comp.Mode = OperatingMode.OpenClose;
                switchMessageId = "door-remote-switch-state-open-close";
                break;
            default:
                throw new InvalidOperationException(
                    $"{nameof(DoorRemoteComponent)} had invalid mode {entity.Comp.Mode}");
        }
        Dirty(entity);
        Popup.PopupClient(Loc.GetString(switchMessageId), entity, args.User);
    }
}
