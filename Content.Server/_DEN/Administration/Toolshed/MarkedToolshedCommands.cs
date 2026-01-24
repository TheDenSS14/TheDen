using System.Linq;
using Content.Server._Floof.Traits.Components;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Objectives.Components;
using Robust.Shared.Toolshed;


namespace Content.Server._DEN.Administration.Toolshed;

[ToolshedCommand, AdminCommand(AdminFlags.Admin)]
internal sealed class MarkedCompCommand : ToolshedCommand
{
    [CommandImplementation("with")]
    public IEnumerable<EntityUid> With([PipedArgument] IEnumerable<EntityUid> input, ObjectiveTypes type) =>
        input.Where(uid => TryComp(uid, out MarkedComponent? marked) && marked.TargetType == type);

    [CommandImplementation("list")]
    public IEnumerable<EntityUid> List([PipedArgument] IEnumerable<EntityUid> input) =>
        input.Where(uid => TryComp(uid, out MarkedComponent? _));

    [CommandImplementation("type")]
    public IEnumerable<string> Type([PipedArgument] IEnumerable<EntityUid> input) =>
        input.Select(GetMarkedSettingsString);

    private string GetMarkedSettingsString(EntityUid input)
    {
        var result = $"Entity {EntityManager.ToPrettyString(input)} has ";

        if (!TryComp(input, out MarkedComponent? marked))
            result += "no marked component.";
        else
            result += $"marked set to {marked.TargetType}";

        return result;
    }
}
