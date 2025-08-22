using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;


namespace Content.Server._DEN.Discord;


public sealed partial class DiscordUserLink
{
    public void InitializeGame()
    {

    }
}

[AdminCommand(AdminFlags.None)]
public sealed class VerifyCommand : IConsoleCommand
{
    public string Command => "verify";
    public string Description => "Verify your discord account with your SS14 account.";
    public string Help => "Usage: verify <code>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entityManager = IoCManager.Resolve<IEntityManager>();
        var discordUserLink = entityManager.System<DiscordUserLink>();

        if (args.Length < 1)
            return;

        discordUserLink.TryGameVerify(args[0]);
    }
}
