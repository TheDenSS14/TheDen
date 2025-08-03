// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Server.Silicons.Borgs.Components;

/// <summary>
/// Server side indicator for a jetpack module. Used as conditional for inserting in canisters.
/// </summary>
[RegisterComponent]
public sealed partial class BorgJetpackComponent : Component
{
    public EntityUid? JetpackUid = null;
}