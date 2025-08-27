// SPDX-FileCopyrightText: 2021 20kdc
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Ray
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Client.Eui;
using Content.Client.Lobby;
using Content.Client.Players.PlayTimeTracking;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Eui;
using Content.Shared.Ghost.Roles;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Client.UserInterface.Systems.Ghost.Controls.Roles
{
    [UsedImplicitly]
    public sealed class GhostRolesEui : BaseEui
    {
        private readonly GhostRolesWindow _window;
        private GhostRoleRulesWindow? _windowRules = null;
        private uint _windowRulesId = 0;

        public GhostRolesEui()
        {
            _window = new GhostRolesWindow();

            _window.OnRoleRequestButtonClicked += info =>
            {
                _windowRules?.Close();

                if (info.Kind == GhostRoleKind.RaffleJoined)
                {
                    SendMessage(new LeaveGhostRoleRaffleMessage(info.Identifier));
                    return;
                }

                _windowRules = new GhostRoleRulesWindow(info.Rules, _ =>
                {
                    SendMessage(new RequestGhostRoleMessage(info.Identifier));

                    // if raffle role, close rules window on request, otherwise do
                    // old behavior of waiting for the server to close it
                    if (info.Kind != GhostRoleKind.FirstComeFirstServe)
                        _windowRules?.Close();
                });
                _windowRulesId = info.Identifier;
                _windowRules.OnClose += () =>
                {
                    _windowRules = null;
                };
                _windowRules.OpenCentered();
            };

            _window.OnRoleFollow += info =>
            {
                SendMessage(new FollowGhostRoleMessage(info.Identifier));
            };

            _window.OnClose += () =>
            {
                SendMessage(new CloseEuiMessage());
            };
        }

        public override void Opened()
        {
            base.Opened();
            _window.OpenCentered();
        }

        public override void Closed()
        {
            base.Closed();
            _window.Close();
            _windowRules?.Close();
        }

        public override void HandleState(EuiStateBase state)
        {
            base.HandleState(state);

            if (state is not GhostRolesEuiState ghostState)
                return;

            // We must save BodyVisible state, so all Collapsible boxes will not close
            // on adding new ghost role.
            // Save the current state of each Collapsible box being visible or not
            _window.SaveCollapsibleBoxesStates();

            // Clearing the container before adding new roles
            _window.ClearEntries();

            var entityManager = IoCManager.Resolve<IEntityManager>();
            var sysManager = entityManager.EntitySysManager;
            var spriteSystem = sysManager.GetEntitySystem<SpriteSystem>();
            var requirementsManager = IoCManager.Resolve<JobRequirementsManager>();
            var characterReqs = entityManager.System<SharedCharacterRequirementsSystem>();
            var prefs = IoCManager.Resolve<IClientPreferencesManager>();
            var protoMan = IoCManager.Resolve<IPrototypeManager>();
            var configManager = IoCManager.Resolve<IConfigurationManager>();

            // TODO: role.Requirements value doesn't work at all as an equality key, this must be fixed
            // Grouping roles
            var groupedRoles = ghostState.GhostRoles.GroupBy(
                role => (role.Name, role.Description, role.Requirements));

            // Add a new entry for each role group
            foreach (var group in groupedRoles)
            {
                var name = group.Key.Name;
                var description = group.Key.Description;
                // ReSharper disable once ReplaceWithSingleAssignment.True
                var hasAccess = true;

                var requirements = group.Key.Requirements ?? new();
                var context = new CharacterRequirementContext(
                    profile: (HumanoidCharacterProfile) (prefs.Preferences?.SelectedCharacter
                        ?? HumanoidCharacterProfile.DefaultWithSpecies()),
                    playtimes: requirementsManager.GetRawPlayTimeTrackers(),
                    whitelisted: requirementsManager.IsWhitelisted());

                if (!characterReqs.CheckRequirementsValid(requirements,
                    context,
                    entityManager,
                    protoMan,
                    configManager))
                    hasAccess = false;

                var reasons = characterReqs.GetReasons(requirements,
                    context,
                    entityManager,
                    protoMan,
                    configManager);
                _window.AddEntry(name, description, hasAccess, characterReqs.GetRequirementsText(reasons), group, spriteSystem);
            }

            // Restore the Collapsible box state if it is saved
            _window.RestoreCollapsibleBoxesStates();

            // Close the rules window if it is no longer needed
            var closeRulesWindow = ghostState.GhostRoles.All(role => role.Identifier != _windowRulesId);
            if (closeRulesWindow)
                _windowRules?.Close();
        }
    }
}
