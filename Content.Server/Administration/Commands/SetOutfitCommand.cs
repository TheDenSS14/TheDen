// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Leo <lzimann@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Timemaster99 <57200767+Timemaster99@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Administration.UI;
using Content.Server.EUI;
using Content.Server.Hands.Systems;
using Content.Server.Preferences.Managers;
using Content.Shared.Access.Components;
using Content.Shared.Administration;
using Content.Shared.Hands.Components;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Content.Shared.Station;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Content.Server.Silicon.IPC;
using Content.Shared.Radio.Components;
using Content.Shared.Cluwne;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class SetOutfitCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entities = default!;

        public string Command => "setoutfit";

        public string Description => Loc.GetString("set-outfit-command-description", ("requiredComponent", nameof(InventoryComponent)));

        public string Help => Loc.GetString("set-outfit-command-help-text", ("command", Command));

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 1)
            {
                shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!int.TryParse(args[0], out var entInt))
            {
                shell.WriteLine(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            var nent = new NetEntity(entInt);

            if (!_entities.TryGetEntity(nent, out var target))
            {
                shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
                return;
            }

            if (!_entities.HasComponent<InventoryComponent>(target))
            {
                shell.WriteLine(Loc.GetString("shell-target-entity-does-not-have-message", ("missing", "inventory")));
                return;
            }

            if (args.Length == 1)
            {
                if (shell.Player is not { } player)
                {
                    shell.WriteError(Loc.GetString("set-outfit-command-is-not-player-error"));
                    return;
                }

                var eui = IoCManager.Resolve<EuiManager>();
                var ui = new SetOutfitEui(nent);
                eui.OpenEui(ui, player);
                return;
            }

            if (!SetOutfit(target.Value, args[1], _entities))
                shell.WriteLine(Loc.GetString("set-outfit-command-invalid-outfit-id-error"));
        }

        public static bool SetOutfit(EntityUid target, string gear, IEntityManager entityManager, Action<EntityUid, EntityUid>? onEquipped = null)
        {
            if (!entityManager.TryGetComponent(target, out InventoryComponent? inventoryComponent))
                return false;

            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            if (!prototypeManager.TryIndex<StartingGearPrototype>(gear, out var startingGear))
                return false;

            HumanoidCharacterProfile? profile = null;
            // Check if we are setting the outfit of a player to respect the preferences
            if (entityManager.TryGetComponent(target, out ActorComponent? actorComponent))
            {
                var userId = actorComponent.PlayerSession.UserId;
                var preferencesManager = IoCManager.Resolve<IServerPreferencesManager>();
                var prefs = preferencesManager.GetPreferences(userId);
                profile = prefs.SelectedCharacter as HumanoidCharacterProfile;

                if (profile != null)
                    startingGear = IoCManager.Resolve<IEntityManager>().System<SharedStationSpawningSystem>().ApplySubGear(startingGear, profile);
            }

            var invSystem = entityManager.System<InventorySystem>();
            if (invSystem.TryGetSlots(target, out var slots))
            {
                foreach (var slot in slots)
                {
                    invSystem.TryUnequip(target, slot.Name, true, true, false, inventoryComponent);
                    var gearStr = startingGear.GetGear(slot.Name, profile);
                    if (gearStr == string.Empty)
                    {
                        continue;
                    }
                    var equipmentEntity = entityManager.SpawnEntity(gearStr, entityManager.GetComponent<TransformComponent>(target).Coordinates);
                    if (slot.Name == "id" &&
                        entityManager.TryGetComponent(equipmentEntity, out PdaComponent? pdaComponent) &&
                        entityManager.TryGetComponent<IdCardComponent>(pdaComponent.ContainedId, out var id))
                    {
                        id.FullName = entityManager.GetComponent<MetaDataComponent>(target).EntityName;
                    }

                    invSystem.TryEquip(target, equipmentEntity, slot.Name, silent: true, force: true, inventory: inventoryComponent);

                    onEquipped?.Invoke(target, equipmentEntity);
                }
            }

            if (entityManager.TryGetComponent(target, out HandsComponent? handsComponent))
            {
                var handsSystem = entityManager.System<HandsSystem>();
                var coords = entityManager.GetComponent<TransformComponent>(target).Coordinates;
                foreach (var prototype in startingGear.Inhand)
                {
                    var inhandEntity = entityManager.SpawnEntity(prototype, coords);
                    handsSystem.TryPickup(target, inhandEntity, checkActionBlocker: false, handsComp: handsComponent);
                }
            }

            if (entityManager.HasComponent<CluwneComponent>(target))
                return true; //Fuck it, nuclear option for not Cluwning an IPC because that causes a crash that SOMEHOW ignores null checks.
            if (entityManager.HasComponent<EncryptionKeyHolderComponent>(target))
            {
                var encryption = entityManager.System<InternalEncryptionKeySpawner>();
                encryption.TryInsertEncryptionKey(target, startingGear, entityManager);
            }
            return true;
        }
    }
}
