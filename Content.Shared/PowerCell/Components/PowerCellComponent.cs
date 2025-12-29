// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 ShadowCommander
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Sailor
// SPDX-FileCopyrightText: 2023 Vasilis
// SPDX-FileCopyrightText: 2023 keronshb
// SPDX-FileCopyrightText: 2025 Shaman
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.PowerCell;

/// <summary>
///     This component enables power-cell related interactions (e.g., entity white-lists, cell sizes, examine, rigging).
///     The actual power functionality is provided by the server-side BatteryComponent.
/// </summary>
[NetworkedComponent]
[RegisterComponent]
public sealed partial class PowerCellComponent : Component
{
    /// <summary>
    ///     How many visual levels a power cell uses to display charge.
    ///     Defaults to 2 unless overridden.
    /// </summary>
    [DataField] // TheDen - Made this like MagazineVisualsComponent where it's variable instead of hardcoded to 2
    public int Levels = 2;
}

[Serializable, NetSerializable]
public enum PowerCellVisuals : byte
{
    ChargeLevel
}
[Serializable, NetSerializable]
public enum PowerCellSlotVisuals : byte
{
    Enabled
}
