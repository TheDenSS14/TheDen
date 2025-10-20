// SPDX-FileCopyrightText: 2022 Paul Ritter
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 c4llv07e
// SPDX-FileCopyrightText: 2025 Solaris
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Access.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.Access;

/// <summary>
///     Contains a list of access tags that are part of this group.
///     Used by <see cref="AccessComponent"/> to avoid boilerplate.
/// </summary>
[Prototype("accessGroup")]
public sealed partial class AccessGroupPrototype : IPrototype, IInheritingPrototype // DEN: I'm making this shit inheriting!!
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The player-visible name of the access level group
    /// </summary>
    [DataField]
    public string? Name { get; set; }

    /// <summary>
    /// The access levels associated with this group
    /// </summary>
    [DataField(required: true)]
    [AlwaysPushInheritance] // DEN - Allow partial inheritance of tags
    public HashSet<ProtoId<AccessLevelPrototype>> Tags = default!;

    // DEN start: I'm making this shit inheriting!!
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<AccessGroupPrototype>))]
    public string[]? Parents { get; }

    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; }
    // End DEN

    public string GetAccessGroupName()
    {
        if (Name is { } name)
            return Loc.GetString(name);

        return ID;
    }
}
