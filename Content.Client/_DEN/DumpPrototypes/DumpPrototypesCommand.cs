using System.IO;
using System.Linq;
using System.Text;
using Content.Shared.Administration;
using Content.Shared.Maps;
using Robust.Client.UserInterface;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Client.DumpPrototypesCommand;

[AnyCommand]
public sealed class DumpPrototypesCommand : IConsoleCommand
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IFileDialogManager _dialogManager = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    public string Command => "dumpprototypes";
    public string Description => Loc.GetString("Dumps all currently loaded entity and tile prototypes into a file.");
    public string Help => ""; // doesn't need it, no arguments

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var file = await _dialogManager.SaveFile(new FileDialogFilters(new FileDialogFilters.Group("txt"))); // Content.Client fails the type check when I try to use json

        var sawmill = _logManager.GetSawmill(Command);

        // while we wait for the dialog, we can do this in the background.

        //var prototypes = _prototypeManager.EnumeratePrototypeKinds(); // possibly use this to dump all prototypes of all kinds?

        var entities = _prototypeManager.EnumeratePrototypes<EntityPrototype>();
        var tiles = _prototypeManager.EnumeratePrototypes<ContentTileDefinition>();

        var dump = new StringBuilder();

        dump.AppendLine("=== Entities ===");

        foreach (var entity in entities)
        {
            dump.AppendLine(entity.ID);
        }

        dump.AppendLine("=== Tiles ===");

        foreach (var tile in tiles)
        {
            dump.AppendLine(tile.ID);
        }

        if (file == null)
        {
            sawmill.Error($"Error when dumping prototypes: Could not create file.");
            return;
        }

        try
        {
            await using var writer = new StreamWriter(file.Value.fileStream);
            writer.Write(dump.ToString());

        }
        catch (Exception exc)
        {
            sawmill.Error($"Error when dumping prototypes: {exc.Message}\n{exc.StackTrace}");
        }
        finally
        {
            await file.Value.fileStream.DisposeAsync();
        }
    }
}