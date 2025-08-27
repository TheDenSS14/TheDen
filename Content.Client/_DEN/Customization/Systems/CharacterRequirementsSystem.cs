
using Content.Client.Lobby;
using Content.Client.Players.PlayTimeTracking;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._DEN.Customization.Systems;

public sealed partial class CharacterRequirementsSystem : SharedCharacterRequirementsSystem
{
    [Dependency] private readonly IClientPreferencesManager _clientPreferences = default!;
    [Dependency] private readonly JobRequirementsManager _requirements = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

    /// <summary>
    ///     Gets the context of the current player, with selected profile, job, whitelist status, and playtimes
    ///     already pre-filled. If the currently-selected character profile is null, a default profile will be
    ///     used instead.
    /// </summary>
    /// <returns>A context associated with the current profile.</returns>
    [PublicAPI]
    public CharacterRequirementContext GetProfileContext(HumanoidCharacterProfile? profile = null)
    {
        if (profile is null)
        {
            var selectedCharacter = _clientPreferences.Preferences?.SelectedCharacter;
            profile = selectedCharacter != null
                ? (HumanoidCharacterProfile) selectedCharacter
                : HumanoidCharacterProfile.DefaultWithSpecies();
        }

        var controller = _userInterface.GetUIController<LobbyUIController>();
        var job = controller.GetPreferredJob(profile);
        var playtimes = _requirements.GetRawPlayTimeTrackers();
        var whitelisted = _requirements.IsWhitelisted();

        return new CharacterRequirementContext(selectedJob: job,
            profile: profile,
            playtimes: playtimes,
            whitelisted: whitelisted);
    }
}
