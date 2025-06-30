// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Ted Lukin
// SPDX-FileCopyrightText: 2025 pheenty
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.CrewMonitoring;

[Serializable, NetSerializable]
public sealed partial class CrewMonitorScanningDoAfterEvent : SimpleDoAfterEvent
{
}
