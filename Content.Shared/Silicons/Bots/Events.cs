// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Actions;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Magic;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Bots;

[ByRefEvent]
public sealed partial class MedibotInjectTargetActionEvent : EntityTargetActionEvent
{ }

[ByRefEvent]
public sealed partial class PlantbotWaterPlantActionEvent : EntityTargetActionEvent
{ }

[ByRefEvent]
public sealed partial class PlantbotRemoveWeedsActionEvent : EntityTargetActionEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantbotWateringDoAfterEvent : SimpleDoAfterEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantbotWeedingDoAfterEvent : SimpleDoAfterEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantbotDrinkingDoAfterEvent : SimpleDoAfterEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class WeldbotWeldEntityDoAfterEvent : SimpleDoAfterEvent
{ }
