// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Radiation.Events;

/// <summary>
///     Raised on entity when it was irradiated
///     by some radiation source.
/// </summary>
public sealed class OnIrradiatedEvent : EntityEventArgs
{
    public readonly float FrameTime;

    public readonly float RadsPerSecond;

    public float TotalRads => RadsPerSecond * FrameTime;

    public OnIrradiatedEvent(float frameTime, float radsPerSecond)
    {
        FrameTime = frameTime;
        RadsPerSecond = radsPerSecond;
    }
}
