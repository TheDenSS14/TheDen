using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.RoundEnd;

[AdminCommand(AdminFlags.Admin)]
public sealed class ToggleHardEndCommand : IConsoleCommand
{
    public string Command { get; } = "togglehardend";
    public string Description { get; } = "Toggles whether or not recall should be allowed after hard end is reached.";
    public string Help { get; } = "togglehardend";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var sysManager = IoCManager.Resolve<IEntitySystemManager>();
        var roundEndSystem = sysManager.GetEntitySystem<RoundEndSystem>();
        roundEndSystem.RespectRoundHardEnd = !roundEndSystem.RespectRoundHardEnd;
        roundEndSystem.UpdateRoundEnd();

        var toggled = roundEndSystem.RespectRoundHardEnd ? "will now" : "will no longer";
        shell.WriteLine($"The round {toggled} end when hard end is reached.");
    }
}
