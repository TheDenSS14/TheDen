// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.ShockCollar;

[RegisterComponent]
public sealed partial class ShockCollarComponent : Component
{
    [DataField]
    public int ShockDamage = 1;

    [DataField]
    public TimeSpan ShockTime = TimeSpan.FromSeconds(2);
}

