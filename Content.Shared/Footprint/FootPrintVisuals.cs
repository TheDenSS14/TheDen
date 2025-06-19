// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.FootPrint;

[Serializable, NetSerializable]
public enum FootPrintVisuals : byte
{
    BareFootPrint,
    ShoesPrint,
    SuitPrint,
    Dragging
}

[Serializable, NetSerializable]
public enum FootPrintVisualState : byte
{
    State,
    Color
}

[Serializable, NetSerializable]
public enum FootPrintVisualLayers : byte
{
    Print
}
