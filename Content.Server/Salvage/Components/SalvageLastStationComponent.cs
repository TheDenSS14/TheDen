// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server.Salvage.Components;

[RegisterComponent]
public sealed partial class SalvageLastStationComponent : Component
{
    [AutoNetworkedField]
    public EntityUid? StationID = null;
}
