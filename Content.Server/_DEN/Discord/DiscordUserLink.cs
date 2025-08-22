using System.Threading.Tasks;
using Content.Server.Discord.DiscordLink;
using NetCord.Gateway;
using Robust.Shared.Network;


namespace Content.Server._DEN.Discord;


/// <summary>
/// This handles linking SS14 and discord account
/// </summary>
public sealed partial class DiscordUserLink : EntitySystem
{
    [Dependency] private readonly DiscordLink _discordLink = default!;

    private Dictionary<NetUserId, ulong> _links = new();
    private HashSet<PendingLink> _pendingLinks = new();
    private HashSet<ulong> _readDisclaimer = new();

    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const int CodeLength = 6;
    private string _combinedApplicableCodeSymbols = Letters + Numbers;

    /// <inheritdoc/>
    public override void Initialize()
    {
        _discordLink.RegisterCommandCallback(OnVerifyCommandRun, "verify");
        _discordLink.RegisterCommandCallback(OnUnverifyCommandRun, "unverify");

        _combinedApplicableCodeSymbols += Letters.ToUpper();
        InitializeGame();
    }

    public bool TryGameVerify(string code)
    {
        // TODO
        return true;
    }

    private string StartVerify(ulong userId)
    {
        var code = GetRandomCode(CodeLength);
        var pendingLink = new PendingLink(userId, code);

        _pendingLinks.RemoveWhere(link => link.DiscordUserId == userId);
        _pendingLinks.Add(pendingLink);

        return code;
    }

    private string GetRandomCode(int length)
    {
        var code = string.Empty;
        var random = new Random();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(0, _combinedApplicableCodeSymbols.Length - 1);
            var codeSymbol = _combinedApplicableCodeSymbols[index];
            code += codeSymbol;
        }

        return code;
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

    private async Task SendDirectMessage(ulong userId, string message)
    {
        if (_discordLink.Client == null)
            return;

        var dm = await _discordLink.Client.Rest.GetDMChannelAsync(userId);
        await dm.SendMessageAsync(message);
    }

    private void OnUnverifyCommandRun(CommandReceivedEventArgs args)
    {
        // TODO
    }
}

public record struct PendingLink
{
    public ulong DiscordUserId { get; init; }
    public string Code { get; init; }

    public PendingLink(ulong discordUserId, string code)
    {
        DiscordUserId = discordUserId;
        Code = code;
    }
}
