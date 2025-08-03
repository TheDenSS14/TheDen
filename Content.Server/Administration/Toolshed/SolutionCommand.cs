// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Syntax;
using Robust.Shared.Toolshed.TypeParsers;
using System.Linq;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Toolshed;

[ToolshedCommand, AdminCommand(AdminFlags.Debug)]
public sealed class SolutionCommand : ToolshedCommand
{
    private SolutionContainerSystem? _solutionContainer;

    [CommandImplementation("get")]
    public SolutionRef? Get([PipedArgument] EntityUid input, string name)
    {
        _solutionContainer ??= GetSys<SolutionContainerSystem>();

        if (_solutionContainer.TryGetSolution(input, name, out var solution))
            return new SolutionRef(solution.Value);

        return null;
    }

    [CommandImplementation("get")]
    public IEnumerable<SolutionRef> Get([PipedArgument] IEnumerable<EntityUid> input, string name)
    {
        return input.Select(x => Get(x, name)).Where(x => x is not null).Cast<SolutionRef>();
    }

    [CommandImplementation("adjreagent")]
    public SolutionRef AdjReagent(
            [PipedArgument] SolutionRef input,
            ProtoId<ReagentPrototype> proto,
            FixedPoint2 amount
        )
    {
        _solutionContainer ??= GetSys<SolutionContainerSystem>();

        if (amount > 0)
        {
            _solutionContainer.TryAddReagent(input.Solution, proto, amount, out _);
        }
        else if (amount < 0)
        {
            _solutionContainer.RemoveReagent(input.Solution, proto, -amount);
        }

        return input;
    }

    [CommandImplementation("adjreagent")]
    public IEnumerable<SolutionRef> AdjReagent(
            [PipedArgument] IEnumerable<SolutionRef> input,
            ProtoId<ReagentPrototype> name,
            FixedPoint2 amount
        )
        => input.Select(x => AdjReagent(x, name, amount));
}

public readonly record struct SolutionRef(Entity<SolutionComponent> Solution)
{
    public override string ToString()
    {
        return $"{Solution.Owner} {Solution.Comp.Solution}";
    }
}
