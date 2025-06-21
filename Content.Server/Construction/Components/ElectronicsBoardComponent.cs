// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Construction.Components;

/// <summary>
/// Used in construction graphs for building wall-mounted electronic devices.
/// </summary>
[RegisterComponent]
public sealed partial class ElectronicsBoardComponent : Component
{
    /// <summary>
    /// The device that is produced when the construction is completed.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Prototype;
}
