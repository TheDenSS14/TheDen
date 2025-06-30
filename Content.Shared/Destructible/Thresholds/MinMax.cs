// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Random;

namespace Content.Shared.Destructible.Thresholds;

[DataDefinition, Serializable]
public partial struct MinMax
{
    [DataField]
    public int Min;

    [DataField]
    public int Max;

    public MinMax(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Next(IRobustRandom random)
    {
        return random.Next(Min, Max + 1);
    }
}
