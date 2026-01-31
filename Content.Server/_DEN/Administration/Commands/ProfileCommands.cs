using System.Linq;
using Content.Server.Administration;
using Content.Server.Humanoid;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration;
using Content.Shared.Preferences;
using Robust.Server.Player;
using Robust.Shared.Console;


namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class ApplyProfileCommand : LocalizedEntityCommands
{
    [Dependency] private readonly HumanoidAppearanceSystem _humanoidAppearanceSystem = null!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly IServerPreferencesManager _prefsManager = null!;

    public override string Command => "applyprofile";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 3)
        {
            shell.WriteError(Help);
            return;
        }

        var applyingTo = args[0];

        if (!_playerManager.TryGetSessionByUsername(applyingTo, out var applyingToSession)
            || applyingToSession.AttachedEntity is not { Valid: true } applyingToEntity)
        {
            var noPlayerFoundMsg = Loc.GetString("cmd-applyprofile-no-player", ("username", applyingTo));

            shell.WriteError(noPlayerFoundMsg);
            shell.WriteError(Help);
            return;
        }

        var characterOwner = args[1];

        if (!_playerManager.TryGetPlayerDataByUsername(characterOwner, out var characterOwnerSession))
        {
            var noPlayerFoundMsg = Loc.GetString("cmd-applyprofile-no-player", ("username", characterOwner));

            shell.WriteError(noPlayerFoundMsg);
            shell.WriteError(Help);
            return;
        }

        var prefs = _prefsManager.GetPreferencesOrNull(characterOwnerSession?.UserId);

        if (prefs == null)
        {
            var noPreferencesFoundMsg = Loc.GetString("cmd-applyprofile-no-preferences", ("username", characterOwner));

            shell.WriteError(noPreferencesFoundMsg);
            shell.WriteError(Help);
            return;
        }

        var indexInput = args[2];

        if (!int.TryParse(indexInput, out var index))
        {
            var invalidIndex = Loc.GetString("cmd-applyprofile-invalid-index", ("index", index));

            shell.WriteError(invalidIndex);
            shell.WriteError(Help);
            return;
        }

        if (!prefs.TryGetProfile(index, out var profile) || profile is not HumanoidCharacterProfile humanoidProfile)
        {
            var invalidIndex = Loc.GetString("cmd-applyprofile-invalid-index", ("index", index));

            shell.WriteError(invalidIndex);
            shell.WriteError(Help);
            return;
        }

        _humanoidAppearanceSystem.LoadProfile(applyingToEntity, humanoidProfile);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromOptions(CompletionHelper.SessionNames());
        }

        if (args.Length == 2)
        {
            return CompletionResult.FromOptions(CompletionHelper.SessionNames());
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Admin)]
public sealed class ListProfileCharactersCommand : LocalizedEntityCommands
{
    [Dependency] private readonly HumanoidAppearanceSystem _humanoidAppearanceSystem = null!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly IServerPreferencesManager _prefsManager = null!;

    public override string Command => "listcharacters";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteError(Help);
            return;
        }

        var characterOwner = args[0];

        if (!_playerManager.TryGetPlayerDataByUsername(characterOwner, out var characterOwnerSession))
        {
            var noPlayerFoundMsg = Loc.GetString("cmd-applyprofile-no-player", ("username", characterOwner));

            shell.WriteError(noPlayerFoundMsg);
            shell.WriteError(Help);
            return;
        }

        var prefs = _prefsManager.GetPreferencesOrNull(characterOwnerSession?.UserId);

        if (prefs == null)
        {
            var noPreferencesFoundMsg = Loc.GetString("cmd-applyprofile-no-preferences", ("username", characterOwner));

            shell.WriteError(noPreferencesFoundMsg);
            shell.WriteError(Help);
            return;
        }

        var compiledCharacters = prefs.Characters.Select(pair => $"{pair.Key}: {pair.Value.Name}");
        shell.WriteLine(string.Join("\n", compiledCharacters));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromOptions(CompletionHelper.SessionNames());
        }

        return CompletionResult.Empty;
    }
}
