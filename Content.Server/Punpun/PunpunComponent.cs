// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Punpun;

[RegisterComponent]
public sealed partial class PunpunComponent : Component
{
    /// How many rounds Punpun will be around for before disappearing with a note
    [DataField]
    public int Lifetime = 14;
}
