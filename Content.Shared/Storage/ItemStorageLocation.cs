// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Storage;

[DataDefinition, Serializable, NetSerializable]
public partial record struct ItemStorageLocation
{
    /// <summary>
    /// The rotation, stored a cardinal direction in order to reduce rounding errors.
    /// </summary>
    [DataField("_rotation")]
    public Direction Direction;

    /// <summary>
    /// The rotation of the piece in storage.
    /// </summary>
    public Angle Rotation
    {
        get => Direction.ToAngle();
        set => Direction = value.GetCardinalDir();
    }

    /// <summary>
    /// Where the item is located in storage.
    /// </summary>
    [DataField]
    public Vector2i Position;

    public ItemStorageLocation(Angle rotation, Vector2i position)
    {
        Rotation = rotation;
        Position = position;
    }

    public bool Equals(ItemStorageLocation? other)
    {
        return Rotation == other?.Rotation &&
               Position == other.Value.Position;
    }
};
