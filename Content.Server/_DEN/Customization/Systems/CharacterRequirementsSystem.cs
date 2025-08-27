using Content.Server.Players.PlayTimeTracking;
using Content.Server.Preferences.Managers;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Players;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._DEN.Customization.Systems;

public sealed partial class CharacterRequirementsSystem : SharedCharacterRequirementsSystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IServerPreferencesManager _prefs = default!;
    [Dependency] private readonly PlayTimeTrackingManager _tracking = default!;

    /// <summary>
    ///     Gets the context of the current player, with selected profile, whitelist status, and playtimes
    ///     already pre-filled. Profile may be null.
    /// </summary>
    /// <returns>A context associated with the current profile.</returns>
    [PublicAPI]
    public CharacterRequirementContext GetProfileContext(ICommonSession player,
        HumanoidCharacterProfile? profile = null)
    {
        if (profile == null)
        {
            var selectedCharacter = _prefs.GetPreferences(player.UserId).SelectedCharacter;
            profile = (HumanoidCharacterProfile?) selectedCharacter;
        }

        var whitelisted = player.ContentData()?.Whitelisted ?? false;

        if (!_tracking.TryGetTrackerTimes(player, out var playtimes))
            playtimes = new();

        return new CharacterRequirementContext(profile: profile,
            playtimes: playtimes,
            whitelisted: whitelisted);
    }

    /// <summary>
    ///     Gets the context of the current player, with selected profile, whitelist status, and playtimes
    ///     already pre-filled. Profile may be null.
    /// </summary>
    /// <returns>A context associated with the current profile.</returns>
    [PublicAPI]
    public CharacterRequirementContext GetProfileContext(NetUserId userId, HumanoidCharacterProfile? profile = null)
        => GetProfileContext(_playerManager.GetSessionById(userId), profile);
}
