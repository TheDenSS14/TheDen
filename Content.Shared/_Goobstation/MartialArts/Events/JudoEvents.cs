// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.MartialArts.Events;
[Serializable, NetSerializable, DataDefinition]
public sealed partial class JudoThrowPerformedEvent : EntityEventArgs;

[Serializable, NetSerializable, DataDefinition]
public sealed partial class JudoEyePokePerformedEvent : EntityEventArgs;


[Serializable, NetSerializable, DataDefinition]
public sealed partial class JudoArmbarPerformedEvent : EntityEventArgs;


[Serializable, NetSerializable, DataDefinition]
public sealed partial class JudoGoldenBlastPerformedEvent : EntityEventArgs;
