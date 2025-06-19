// SPDX-FileCopyrightText: 2023 Colin-Tel <113523727+Colin-Tel@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Laundry;

[RegisterComponent]
public partial class SharedWashingMachineComponent : Component { } //Hi, I'm no coder but the word "partial" used to be "sealed" o3o

[Serializable, NetSerializable]
public enum WashingMachineVisualState : byte
{
    Broken,
}
