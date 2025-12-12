using Content.Shared.DoAfter;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.BluespacePlushiePatch.Events;


[Serializable, NetSerializable]
public sealed partial class BluespacePlushiePatchDoAfterEvent : SimpleDoAfterEvent;
