// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Abilities.Chitinid;


[RegisterComponent]
public sealed partial class BlockInjectionComponent : Component
{
    [DataField]
    public string BlockReason { get; set; } = string.Empty;
}
