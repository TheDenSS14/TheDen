// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Revolutionary.Components;

[Serializable, NetSerializable]
public sealed partial class RevolutionaryConverterDoAfterEvent : SimpleDoAfterEvent
{
}

[RegisterComponent, NetworkedComponent]
public sealed partial class RevolutionaryConverterComponent : Component
{
    [DataField]
    public TimeSpan ConversionDuration { get; set; }
}
