// SPDX-FileCopyrightText: 2024 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.RCD;

[Serializable, NetSerializable]
public sealed class RCDSystemMessage : BoundUserInterfaceMessage
{
    public ProtoId<RCDPrototype> ProtoId;

    public RCDSystemMessage(ProtoId<RCDPrototype> protoId)
    {
        ProtoId = protoId;
    }
}

[Serializable, NetSerializable]
public sealed class RCDConstructionGhostRotationEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;
    public readonly Direction Direction;

    public RCDConstructionGhostRotationEvent(NetEntity netEntity, Direction direction)
    {
        NetEntity = netEntity;
        Direction = direction;
    }
}

[Serializable, NetSerializable]
public sealed class RCDConstructionGhostFlipEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;
    public readonly bool UseMirrorPrototype;
    public RCDConstructionGhostFlipEvent(NetEntity netEntity, bool useMirrorPrototype)
    {
        NetEntity = netEntity;
        UseMirrorPrototype = useMirrorPrototype;
    }
}

[Serializable, NetSerializable]
public enum RcdUiKey : byte
{
    Key
}
