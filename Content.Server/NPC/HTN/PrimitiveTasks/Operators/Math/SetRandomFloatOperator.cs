// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.Random;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Math;

/// <summary>
/// Sets a random float from MinAmount to MaxAmount to blackboard
/// </summary>
public sealed partial class SetRandomFloatOperator : HTNOperator
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [DataField(required: true)]
    public string TargetKey = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxAmount = 1f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MinAmount = 0f;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (
            true,
            new Dictionary<string, object>
            {
                { TargetKey, _random.NextFloat(MinAmount, MaxAmount) }
            }
        );
    }
}
