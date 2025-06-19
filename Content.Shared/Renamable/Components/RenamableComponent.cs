// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Renamable.EntitySystems;
using Robust.Shared.Serialization;


namespace Content.Shared.Renamable.Components;

[Serializable, NetSerializable]
public enum SharedRenamableInterfaceKey
{
    Key
}

[RegisterComponent, Access(typeof(SharedRenamableSystem))]
public sealed partial class RenamableComponent : Component
{

}
