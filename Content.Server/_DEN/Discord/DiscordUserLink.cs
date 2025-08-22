using System.Linq;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.Discord.DiscordLink;
using NetCord.Gateway;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Utility;


namespace Content.Server._DEN.Discord;


/// <summary>
/// This handles linking SS14 and discord accounts.
/// </summary>
public sealed partial class DiscordUserLink : EntitySystem
{
    [Dependency] private readonly DiscordLink _discordLink = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ServerDbManager _db = default!;

    private HashSet<ActiveDiscordLink> _links = new();
    private HashSet<PendingLink> _pendingLinks = new();
    private HashSet<ulong> _readDisclaimer = new();

    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const int CodeLength = 6;
    private string _combinedApplicableCodeSymbols = Letters + Numbers;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;
        _discordLink.RegisterCommandCallback(OnVerifyCommandRun, "verify");
        _discordLink.RegisterCommandCallback(OnUnverifyCommandRun, "unverify");

        _combinedApplicableCodeSymbols += Letters.ToUpper();
        InitializeGame();
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _playerManager.PlayerStatusChanged -= OnPlayerStatusChanged;
    }

    public bool TryGameVerify(NetUserId userId, string code)
    {
        if (_pendingLinks.All(link => link.Code != code))
            return false;

        var pendingCode = _pendingLinks.First(link => link.Code == code);
        _pendingLinks.Remove(pendingCode);

        _links.Add(new(userId, pendingCode.DiscordUserId));
        UpdatePlayerLink(userId, pendingCode.DiscordUserId);
        return true;
    }

    private void OnVerifyCommandRun(CommandReceivedEventArgs args)
    {
        if (args.Arguments.StartsWith("confirm") && _readDisclaimer.Contains(args.Message.Author.Id))
        {
            OnConfirmationReceived(args);
            return;
        }

        args.Message.ReplyAsync("# Disclaimer\nBy linking your account to the game, " +
            "you understand that we are storing a reference between your discord account and your SS14 account. " +
            "If you do not wish to have that connection, please stop here.\n\nTo confirm, please type .verify confirm\nYou can opt out at any time with .unverify.");
        _readDisclaimer.Add(args.Message.Author.Id);
    }

    private async void OnConfirmationReceived(CommandReceivedEventArgs args)
    {
        var code = StartVerify(args.Message.Author.Id);

        Task.Run(async () =>
        {
            await args.Message.ReplyAsync("You should have received a code in your direct messages with me. " +
                "If you did not, re-run the command after lowering your messaging restrictions.");
            await SendDirectMessage(args.Message.Author.Id,
                $"On the game server, type ``verify {code}`` to verify your discord account.");
        });
    }

    private void OnUnverifyCommandRun(CommandReceivedEventArgs args)
    {
        var authorId = args.Message.Author.Id;
        args.Message.ReplyAsync("Done!");

        if (_links.Any(link => link.DiscordUserId != authorId))
            return;

        _links.RemoveWhere(link => link.DiscordUserId == authorId);
        UpdatePlayerLink(authorId, null);
    }
}
