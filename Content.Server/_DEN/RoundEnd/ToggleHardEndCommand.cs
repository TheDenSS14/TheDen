using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.RoundEnd;

[AdminCommand(AdminFlags.Admin)]
public sealed class ToggleHardEndCommand : IConsoleCommand
{
    [Dependency] private readonly RoundEndSystem _roundEndSystem = default!;

    public string Command { get; } = "togglehardend";
    public string Description { get; } = "Toggles whether or not recall should be allowed after hard end is reached.";
    public string Help { get; } = "togglehardend";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        _roundEndSystem.RespectRoundHardEnd = !_roundEndSystem.RespectRoundHardEnd;

        if (_roundEndSystem.RespectRoundHardEnd)
            _roundEndSystem.RestartHardEndWarning();
        else
            _roundEndSystem.CancelHardEndWarning();

        var toggled = _roundEndSystem.RespectRoundHardEnd ? "will now " : "will no longer ";
        shell.WriteLine($"The round {toggled} end when hard end is reached.");
    }
}
