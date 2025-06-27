// SPDX-FileCopyrightText: 2025 Baptr0b0t
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Ted Lukin
// SPDX-FileCopyrightText: 2025 pheenty
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server.RelayedDeathrattle;

[RegisterComponent]
public sealed partial class RelayedDeathrattleComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? Target;

    [DataField]
    public LocId CritMessage = "deathrattle-implant-critical-message";

    [DataField]
    public LocId DeathMessage = "deathrattle-implant-dead-message";

}
