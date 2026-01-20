// SPDX-FileCopyrightText: 2019 PrPleGoo
// SPDX-FileCopyrightText: 2020 Alex S
// SPDX-FileCopyrightText: 2020 ColdAutumnRain
// SPDX-FileCopyrightText: 2020 FL-OZ
// SPDX-FileCopyrightText: 2020 Jackson Lewis
// SPDX-FileCopyrightText: 2020 L.E.D
// SPDX-FileCopyrightText: 2020 Memory
// SPDX-FileCopyrightText: 2020 Remie Richards
// SPDX-FileCopyrightText: 2020 ShadowCommander
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2020 adrian
// SPDX-FileCopyrightText: 2020 boiled water
// SPDX-FileCopyrightText: 2020 boiled-water-tsar
// SPDX-FileCopyrightText: 2020 chairbender
// SPDX-FileCopyrightText: 2020 py01
// SPDX-FileCopyrightText: 2021 20kdc
// SPDX-FileCopyrightText: 2021 AJCM-git
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Daniel Castro Razo
// SPDX-FileCopyrightText: 2021 Galactic Chimp
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2021 Swept
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2021 bryce0110
// SPDX-FileCopyrightText: 2021 collinlunn
// SPDX-FileCopyrightText: 2021 metalgearsloth
// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 Chief-Engineer
// SPDX-FileCopyrightText: 2022 Chris V
// SPDX-FileCopyrightText: 2022 Jacob Tong
// SPDX-FileCopyrightText: 2022 JustinTime
// SPDX-FileCopyrightText: 2022 Júlio César Ueti
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Moony
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Ed
// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Numerics;
using Content.Shared.Physics;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Storage.Components;

[NetworkedComponent]
public abstract partial class SharedEntityStorageComponent : Component
{
    public readonly float MaxSize = 1.0f; // maximum width or height of an entity allowed inside the storage.

    public static readonly TimeSpan InternalOpenAttemptDelay = TimeSpan.FromSeconds(0.5);
    public TimeSpan NextInternalOpenAttempt;

    /// <summary>
    ///     Collision masks that get removed when the storage gets opened.
    /// </summary>
    public readonly int MasksToRemove = (int) (
        CollisionGroup.MidImpassable |
        CollisionGroup.HighImpassable |
        CollisionGroup.LowImpassable);

    /// <summary>
    ///     Collision masks that were removed from ANY layer when the storage was opened;
    /// </summary>
    [DataField]
    public int RemovedMasks;

    /// <summary>
    /// The total amount of items that can fit in one entitystorage
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Capacity = 30;

    /// <summary>
    /// Whether or not the entity still has collision when open
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IsCollidableWhenOpen;

    /// <summary>
    /// If true, it opens the storage when the entity inside of it moves
    /// If false, it prevents the storage from opening when the entity inside of it moves.
    /// This is for objects that you want the player to move while inside, like large cardboard boxes, without opening the storage.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool OpenOnMove = true;

    //The offset for where items are emptied/vacuumed for the EntityStorage.
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Vector2 EnteringOffset = new(0, 0);

    //The collision groups checked, so that items are depositied or grabbed from inside walls.
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public CollisionGroup EnteringOffsetCollisionFlags = CollisionGroup.Impassable | CollisionGroup.MidImpassable;

    /// <summary>
    /// How close you have to be to the "entering" spot to be able to enter
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EnteringRange = 0.18f;

    /// <summary>
    /// If true, there may be mobs inside the container, even if the container is an Item
    /// </summary>
    [DataField]
    public bool ItemCanStoreMobs = false;

    /// <summary>
    /// Whether or not to show the contents when the storage is closed
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool ShowContents;

    /// <summary>
    /// Whether or not light is occluded by the storage
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool OccludesLight = true;

    /// <summary>
    /// Whether or not all the contents stored should be deleted with the entitystorage
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool DeleteContentsOnDestruction;

    /// <summary>
    /// Whether or not the container is sealed and traps air inside of it
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Airtight;

    /// <summary>
    /// Whether or not the entitystorage is open or closed
    /// </summary>
    [DataField]
    public bool Open;

    /// <summary>
    /// The sound made when closed
    /// </summary>
    [DataField]
    public SoundSpecifier CloseSound = new SoundPathSpecifier("/Audio/Effects/closetclose.ogg");

    /// <summary>
    /// The sound made when open
    /// </summary>
    [DataField]
    public SoundSpecifier OpenSound = new SoundPathSpecifier("/Audio/Effects/closetopen.ogg");

    /// <summary>
    ///     Whitelist for what entities are allowed to be inserted into this container. If this is not null, the
    ///     standard requirement that the entity must be an item or mob is waived.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// The contents of the storage
    /// </summary>
    [ViewVariables]
    public Container Contents = default!;
}

[Serializable, NetSerializable]
public sealed class EntityStorageComponentState : ComponentState
{
    public bool Open;

    public int Capacity;

    public bool IsCollidableWhenOpen;

    public bool OpenOnMove;

    public float EnteringRange;

    public TimeSpan NextInternalOpenAttempt;

    public EntityStorageComponentState(bool open, int capacity, bool isCollidableWhenOpen, bool openOnMove, float enteringRange, TimeSpan nextInternalOpenAttempt)
    {
        Open = open;
        Capacity = capacity;
        IsCollidableWhenOpen = isCollidableWhenOpen;
        OpenOnMove = openOnMove;
        EnteringRange = enteringRange;
        NextInternalOpenAttempt = nextInternalOpenAttempt;
    }
}

[ByRefEvent]
public record struct InsertIntoEntityStorageAttemptEvent(bool Cancelled = false);

[ByRefEvent]
public record struct StoreMobInItemContainerAttemptEvent(bool Handled, bool Cancelled = false);

[ByRefEvent]
public record struct StorageOpenAttemptEvent(EntityUid User, bool Silent, bool Cancelled = false);

[ByRefEvent]
public readonly record struct StorageBeforeOpenEvent;

[ByRefEvent]
public readonly record struct StorageAfterOpenEvent;

[ByRefEvent]
public record struct StorageCloseAttemptEvent(EntityUid? User, bool Cancelled = false);

[ByRefEvent]
public readonly record struct StorageBeforeCloseEvent(HashSet<EntityUid> Contents, HashSet<EntityUid> BypassChecks);

[ByRefEvent]
public readonly record struct StorageAfterCloseEvent;
