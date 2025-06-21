// SPDX-FileCopyrightText: 2025 RadsammyT <32146976+RadsammyT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.Cards.Hand;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class CardHandComponent : Component
{
    [DataField]
    public float Angle = 120f;

    [DataField]
    public float XOffset = 0.5f;

    [DataField]
    public float Scale = 1;

    [DataField]
    public int CardLimit = 10;

    [DataField]
    public bool Flipped = false;
}


[Serializable, NetSerializable]
public enum CardUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class CardHandDrawMessage(NetEntity card) : BoundUserInterfaceMessage
{
    public NetEntity Card = card;
}
