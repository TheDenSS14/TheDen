// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Goobstation.Flashbang;

[RegisterComponent]
public sealed partial class FlashbangComponent : Component
{
    [DataField]
    public float StunTime = 2f;

    [DataField]
    public float KnockdownTime = 10f;
}
