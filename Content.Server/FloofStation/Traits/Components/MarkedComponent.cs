// SPDX-FileCopyrightText: 2025 Memeji Dankiri
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Objectives.Components;

namespace Content.Server._Floof.Traits.Components;

/// <summary>
///     Marks this player as eligible for being the target of
///     chosen types of antagonist objectives.
/// </summary>
[RegisterComponent]
public sealed partial class MarkedComponent : Component
{
    [DataField, ViewVariables]
    public ObjectiveTypes TargetType;
}
