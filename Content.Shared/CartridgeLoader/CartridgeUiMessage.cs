// SPDX-FileCopyrightText: 2022 Julian Giebel
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 deltanedas
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader;

[Serializable, NetSerializable]
public sealed class CartridgeUiMessage : BoundUserInterfaceMessage
{
    public CartridgeMessageEvent MessageEvent;

    public CartridgeUiMessage(CartridgeMessageEvent messageEvent)
    {
        MessageEvent = messageEvent;
    }
}

[Serializable, NetSerializable]
public abstract class CartridgeMessageEvent : EntityEventArgs
{
    [NonSerialized]
    public EntityUid User;
    public NetEntity LoaderUid;

    [NonSerialized]
    public EntityUid Actor;
}
