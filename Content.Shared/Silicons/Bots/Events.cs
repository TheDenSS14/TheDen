using Content.Shared.Actions;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Magic;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Bots;

[ByRefEvent]
public sealed partial class MedibotInjectTargetEvent : EntityTargetActionEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantBotWateringDoAfterEvent : SimpleDoAfterEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantBotWeedingDoAfterEvent : SimpleDoAfterEvent
{ }

[ByRefEvent]
[Serializable, NetSerializable]
public sealed partial class PlantBotDrinkingDoAfterEvent : SimpleDoAfterEvent
{ }
