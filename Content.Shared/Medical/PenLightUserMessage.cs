// SPDX-FileCopyrightText: 2024 SleepyScarecrow <136123749+SleepyScarecrow@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Medical;
[Serializable, NetSerializable]
public sealed class PenLightUserMessage : BoundUserInterfaceMessage
{
    public readonly NetEntity? TargetEntity;
    public bool? Blind;
    public bool? Drunk;
    public bool? EyeDamage;
    public bool? Healthy;
    public bool? SeeingRainbows;

    public PenLightUserMessage(NetEntity? targetEntity, bool? blind, bool? drunk, bool? eyeDamage, bool? healthy, bool? seeingRainbows)
    {
        TargetEntity = targetEntity;
        Blind = blind;
        Drunk = drunk;
        EyeDamage = eyeDamage;
        Healthy = healthy;
        SeeingRainbows = seeingRainbows;
    }
}

