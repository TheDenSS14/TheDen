// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.Lobby;
using Content.Client.Players.PlayTimeTracking;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Player;

namespace Content.Client._DEN.Customization.Systems;

public sealed partial class CharacterRequirementsSystem : SharedCharacterRequirementsSystem
{
    [Dependency] private readonly IClientPreferencesManager _clientPreferences = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly JobRequirementsManager _requirements = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

    /// <summary>
    ///     Gets the context of the current player, with selected profile, job, whitelist status, and playtimes
    ///     already pre-filled. If the currently-selected character profile is null, a default profile will be
    ///     used instead.
    /// </summary>
    /// <param name="profile">An optional parameter for a profile to use instead of the player's.</param>
    /// <param name="useCharacter">If profile is null, we use the profile of the current character instead.</param>
    /// <returns>A context associated with the current profile.</returns>
    [PublicAPI]
    public CharacterRequirementContext GetProfileContext(HumanoidCharacterProfile? profile = null,
        bool useCharacter = false)
    {
        var entity = _player.LocalSession?.AttachedEntity;
        if (profile is null)
        {
            if (useCharacter)
            {
                if (entity is not null)
                    TryGetProfile(entity.Value, out profile);
            }
            else
            {
                var selectedCharacter = _clientPreferences.Preferences?.SelectedCharacter;
                if (selectedCharacter is not null)
                    profile = (HumanoidCharacterProfile) selectedCharacter;
            }

            profile ??= HumanoidCharacterProfile.DefaultWithSpecies();
        }

        var controller = _userInterface.GetUIController<LobbyUIController>();
        var job = controller.GetPreferredJob(profile);
        var playtimes = _requirements.GetRawPlayTimeTrackers();
        var whitelisted = _requirements.IsWhitelisted();

        return new CharacterRequirementContext(selectedJob: job,
            profile: profile,
            playtimes: playtimes,
            whitelisted: whitelisted,
            entity: entity);
    }
}
