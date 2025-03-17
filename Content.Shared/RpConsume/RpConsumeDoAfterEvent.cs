using Content.Shared.DoAfter;
using Robust.Shared.Serialization;


namespace Content.Shared.RpConsume;

[Serializable, NetSerializable]
public sealed partial class RpConsumeDoAfterEvent : SimpleDoAfterEvent { }
