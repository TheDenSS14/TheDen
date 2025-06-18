// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Biscuit;

namespace Content.Server._DV.Biscuit;

[RegisterComponent]
public sealed partial class BiscuitComponent : SharedBiscuitComponent
{
    [DataField]
    public bool Cracked { get; set; }
}
