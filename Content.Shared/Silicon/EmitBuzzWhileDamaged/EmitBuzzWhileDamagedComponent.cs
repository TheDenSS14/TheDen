// SPDX-FileCopyrightText: 2024 Timemaster99 <57200767+Timemaster99@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Shared.Silicon.EmitBuzzWhileDamaged;

/// <summary>
///     This is used for controlling the cadence of the buzzing emitted by EmitBuzzOnCritSystem.
///     This component is used by mechanical species that can get to critical health.
/// </summary>
[RegisterComponent]
public sealed partial class EmitBuzzWhileDamagedComponent : Component
{
    [DataField]
    public TimeSpan BuzzPopupCooldown = TimeSpan.FromSeconds(8);

    [ViewVariables]
    public TimeSpan LastBuzzPopupTime;

    [DataField]
    public float CycleDelay = 2.0f;

    [ViewVariables]
    public float AccumulatedFrametime;

    [DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("buzzes");
}
