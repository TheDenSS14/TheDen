// SPDX-FileCopyrightText: 2025 Baptr0b0t
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Ted Lukin
// SPDX-FileCopyrightText: 2025 pheenty
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Whitelist;

namespace Content.Shared.CrewMonitoring;

[RegisterComponent]
public sealed partial class CrewMonitorScanningComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public List<EntityUid> ScannedEntities = [];

    [DataField]
    public TimeSpan DoAfterTime = TimeSpan.FromSeconds(8);

    [DataField]
    public bool ApplyDeathrattle = true;

    [DataField]
    public EntityWhitelist Whitelist = new ();
}
