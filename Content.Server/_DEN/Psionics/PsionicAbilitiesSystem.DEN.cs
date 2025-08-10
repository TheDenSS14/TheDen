using System.Linq;
using Content.Server.Objectives.Commands;
using Content.Shared.Objectives.Components;
using Content.Shared.Prototypes;
using Content.Shared.Psionics;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;


namespace Content.Server.Abilities.Psionics;

public sealed partial class PsionicAbilitiesSystem
{
    private void CreateCompletions(PrototypesReloadedEventArgs unused)
    {
        CreateCompletions();
    }

    /// <summary>
    /// Get all objective prototypes by their IDs.
    /// This is used for completions in <see cref="PsionicsCommands"/>
    /// </summary>
    public List<CompletionOption> PsionicPowers()
    {
        if (_psionicPowers == null)
            CreateCompletions();

        return _psionicPowers!;
    }

    private void CreateCompletions()
    {
        var powers = new List<CompletionOption>();
        foreach (var power in _prototypeManager.EnumeratePrototypes<PsionicPowerPrototype>())
        {
            var powerCompletion = new CompletionOption(power.ID, power.Name);
            powers.Add(powerCompletion);
        }

        _psionicPowers = powers;
    }
}
