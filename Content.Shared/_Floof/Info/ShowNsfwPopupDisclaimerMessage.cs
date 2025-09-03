// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;


namespace Content.Shared.FloofStation.Info;


/// <summary>
///     Sent server->client to command the client to open an NSFW content disclaimer dialog.
/// </summary>
[Serializable, NetSerializable]
public sealed class ShowNsfwPopupDisclaimerMessage : EntityEventArgs;

/// <summary>
///     Client responded to the popup disclaimer.
/// </summary>
public sealed class PopupDisclaimerResponseMessage : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Command;

    public bool Response { get; set; }

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        Response = buffer.ReadBoolean();
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(Response);
    }

    public override NetDeliveryMethod DeliveryMethod => NetDeliveryMethod.ReliableUnordered;
}
