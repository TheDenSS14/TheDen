using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Redial;


public sealed class MsgRequestRedial : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Command;

    public string RedialAddress = string.Empty;
    public string RedialMessage = string.Empty;

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        RedialAddress = buffer.ReadString();
        RedialMessage = buffer.ReadString();
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
       buffer.Write(RedialAddress);
       buffer.Write(RedialMessage);
    }

    public override NetDeliveryMethod DeliveryMethod => NetDeliveryMethod.ReliableOrdered;
}
