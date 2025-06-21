// SPDX-FileCopyrightText: 2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.MassMedia.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.MassMedia.Components;

[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(NewsSystem))]
public sealed partial class NewsWriterComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool PublishEnabled;

    [ViewVariables(VVAccess.ReadWrite), DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextPublish;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float PublishCooldown = 20f;

    [DataField]
    public SoundSpecifier NoAccessSound = new SoundPathSpecifier("/Audio/Machines/airlock_deny.ogg");

    [DataField]
    public SoundSpecifier ConfirmSound = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg");
}
