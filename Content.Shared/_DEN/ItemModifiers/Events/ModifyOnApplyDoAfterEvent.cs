using Content.Shared.DoAfter;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.ItemModifiers.Events;


[Serializable, NetSerializable]
public sealed partial class ModifyOnApplyDoAfterEvent : SimpleDoAfterEvent;
