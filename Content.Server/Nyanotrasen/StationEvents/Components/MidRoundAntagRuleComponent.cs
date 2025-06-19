// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.StationEvents.Events;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(MidRoundAntagRule))]
public sealed partial class MidRoundAntagRuleComponent : Component
{
    /// <summary>
    /// Spawner to create at a random mid round antag marker.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Spawner = string.Empty;
}
