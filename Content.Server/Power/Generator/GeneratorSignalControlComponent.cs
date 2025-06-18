// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SpeltIncorrectyl <66873282+SpeltIncorrectyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DeviceLinking;
using Content.Shared.Power.Generator;
using Robust.Shared.Prototypes;

namespace Content.Server.Power.Generator;

/// <summary>
/// When attached to an entity with <see cref="FuelGeneratorComponent"/> it will allow the signal network to exert control over the generator.
/// </summary>
[RegisterComponent]
public sealed partial class GeneratorSignalControlComponent: Component
{
    /// <summary>
    /// The port that should be invoked when turning the generator on.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> OnPort = "On";

    /// <summary>
    /// The port that should be invoked when turning the generator off.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> OffPort = "Off";

    /// <summary>
    /// The port that should be invoked when toggling the generator.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> TogglePort = "Toggle";
}
