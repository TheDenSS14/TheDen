// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Shitmed.Targeting;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Targeting.Events;

[Serializable, NetSerializable]
public sealed class TargetChangeEvent : EntityEventArgs
{
    public NetEntity Uid { get; }
    public TargetBodyPart BodyPart { get; }
    public TargetChangeEvent(NetEntity uid, TargetBodyPart bodyPart)
    {
        Uid = uid;
        BodyPart = bodyPart;
    }
}

[Serializable, NetSerializable]
public sealed class TargetIntegrityChangeEvent : EntityEventArgs
{
    public NetEntity Uid { get; }
    public bool RefreshUi { get; }
    public TargetIntegrityChangeEvent(NetEntity uid, bool refreshUi = true)
    {
        Uid = uid;
        RefreshUi = refreshUi;
    }
}
