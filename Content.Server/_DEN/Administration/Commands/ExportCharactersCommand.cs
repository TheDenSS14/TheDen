
using System.Threading.Tasks;
using Content.Server.Administration;
using Content.Server.Humanoid;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration;
using Content.Shared.Preferences;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Host)]
public sealed class ExportCharactersCommand : IConsoleCommand
{
    public const string CommandName = "exportcharacters";

    public string Command => CommandName;
    public string Description => "Exports the characters of a given player as .yml files. The output must be retrieved from the server files.";
    public string Help => $"Usage: {Command} <playerUserId OR playerUsername>";

    private const string BaseFolder = "ExportedCharacters";
    private const string FileSuffix = ".yml";

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var arg = "";
        Guid exportedPlayer;

        switch (args.Length)
        {
            case 1 when Guid.TryParse(args[0], out exportedPlayer):
                arg = args[0];
                break;
            case 1:
                var locator = IoCManager.Resolve<IPlayerLocator>();
                var dbGuid = await locator.LookupIdByNameAsync(args[0]);

                if (dbGuid == null)
                {
                    shell.WriteError($"Unable to find {args[0]} netuserid");
                    return;
                }

                arg = args[0];
                exportedPlayer = dbGuid.UserId;
                break;
            default:
                shell.WriteError($"Invalid arguments.\n{Help}");
                return;
        }

        var success = await ExportPlayerCharacters(exportedPlayer, arg);
        if (success)
            shell.WriteLine($"Successfully exported characters for {arg} - please check server data!");
        else
            shell.WriteError($"Failed to export characters for {arg}.");
    }

    private async Task<bool> ExportPlayerCharacters(Guid player, string playerDirectory)
    {
        var serverPrefs = IoCManager.Resolve<IServerPreferencesManager>();
        var resource = IoCManager.Resolve<IResourceManager>();
        var entMan = IoCManager.Resolve<IEntityManager>();
        var humanoidAppearance = entMan.System<HumanoidAppearanceSystem>();

        var prefs = await serverPrefs.GetUserPreferences(player, new());
        if (prefs is null)
            return false;

        var baseDirectory = BaseFolder + ResPath.Separator + playerDirectory;
        var basePath = new ResPath(baseDirectory);
        resource.UserData.CreateDir(basePath.ToRootedPath());

        foreach (var character in prefs.Characters)
        {
            if (character.Value is HumanoidCharacterProfile profile)
                ExportCharacter(baseDirectory, character.Key, profile, humanoidAppearance, resource);
        }

        return true;
    }

    private static void ExportCharacter(string baseDirectory,
        int slot,
        HumanoidCharacterProfile profile,
        HumanoidAppearanceSystem appearance,
        IResourceManager resource)
    {
        var export = appearance.ToDataNode(profile);
        var directory = baseDirectory + ResPath.Separator;
        var prefix = slot + "_";
        var characterName = profile.Name;
        var path = new ResPath(directory + prefix + characterName + FileSuffix);

        using var writer = resource.UserData.OpenWriteText(path.ToRootedPath());
        export.Write(writer);
    }
}
