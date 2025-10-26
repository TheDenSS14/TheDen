using Robust.Shared.Network;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Bed.Cryostorage.Components;

[Serializable, NetSerializable]
public sealed class IgnoreCryoMessage : EntityEventArgs
{
    public bool Ignore { get; }

    public IgnoreCryoMessage(bool ignore)
    {
        Ignore = ignore;
    }
}
