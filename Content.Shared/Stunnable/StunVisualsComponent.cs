// SPDX-FileCopyrightText: 2025 Princess Cheeseballs
//
// SPDX-License-Identifier: AGPL-3.0-or-later

﻿using Robust.Shared.Utility;

namespace Content.Shared.Stunnable;

/// <summary>
/// This is used to listen to incoming events from the AppearanceSystem
/// </summary>
[RegisterComponent]
public sealed partial class StunVisualsComponent : Component
{
    [DataField]
    public ResPath StarsPath = new ("Mobs/Effects/stunned.rsi");

    [DataField]
    public string State = "stunned";
}
