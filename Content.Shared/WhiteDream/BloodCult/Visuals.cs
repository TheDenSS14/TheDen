// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.WhiteDream.BloodCult;

[Serializable, NetSerializable]
public enum SoulShardVisualState : byte
{
    HasMind,
    Blessed,
    Sprite,
    Glow
}

[Serializable, NetSerializable]
public enum ConstructVisualsState : byte
{
    Transforming,
    Sprite,
    Glow
}

[Serializable, NetSerializable]
public enum GenericCultVisuals : byte
{
    State, // True or False
    Layer
}

[Serializable, NetSerializable]
public enum PylonVisuals : byte
{
    Activated,
    Layer
}

[Serializable, NetSerializable]
public enum PentagramKey
{
    Key
}
