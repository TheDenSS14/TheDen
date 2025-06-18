// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Shitmed.BodyEffects;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentPause]
public sealed partial class BodyPartEffectComponent : Component
{
    /// <summary>
    ///     The components that are active on the part and will be refreshed every 5s
    /// </summary>
    [DataField]
    public ComponentRegistry Active = new();

    /// <summary>
    ///     How long to wait between each refresh.
    ///     Effects can only last at most this long once the organ is removed.
    /// </summary>
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(5);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextUpdate = TimeSpan.Zero;
}