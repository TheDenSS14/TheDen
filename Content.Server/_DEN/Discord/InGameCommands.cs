using System.Linq;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Discord.DiscordLink;
using Content.Server.Mind;
using Content.Shared.Administration;
using NetCord;
using Robust.Server.Player;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord;


/// <summary>
/// This handles discord commands that call game code.
/// </summary>
public sealed partial class InGameCommands : EntitySystem
{
    [Dependency] private readonly AdminSystem _adminSystem = default!;
    [Dependency] private readonly BwoinkSystem _bwoinkSystem = default!;
    [Dependency] private readonly DiscordLink _discordLink = default!;
    [Dependency] private readonly DiscordUserLink _userLink = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;
    [Dependency] private readonly IAdminManager _adminManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        // Located in partial classes
        _discordLink.RegisterCommandCallback(OnAdminListCommandRun, "adminwho");
        _discordLink.RegisterCommandCallback(OnCharactersCommandRun, "characters");
        _discordLink.RegisterCommandCallback(OnPlayersCommandRun, "players");
        _discordLink.RegisterCommandCallback(OnSendMessageCommand, "sendmessage");
    }
}
