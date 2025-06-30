// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Blitz <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Server.RandomMetadata;

/// <summary>
///     Randomizes the description and/or the name for an entity by creating it from list of dataset prototypes or strings.
/// </summary>
[RegisterComponent]
public sealed partial class RandomMetadataComponent : Component
{
    [DataField("descriptionSegments")]
    public List<ProtoId<LocalizedDatasetPrototype>>? DescriptionSegments;

    [DataField("nameSegments")]
    public List<ProtoId<LocalizedDatasetPrototype>>? NameSegments;

    /// <summary>
    /// LocId of the formatting string to use to assemble the <see cref="NameSegments"/> into the entity's name.
    /// Segments will be passed to the localization system with this string as variables named $part0, $part1, $part2, etc.
    /// </summary>
    [DataField]
    public LocId NameFormat = "random-metadata-name-format-default";

    /// <summary>
    /// LocId of the formatting string to use to assemble the <see cref="DescriptionSegments"/> into the entity's description.
    /// Segments will be passed to the localization system with this string as variables named $part0, $part1, $part2, etc.
    /// </summary>
    [DataField]
    public LocId DescriptionFormat = "random-metadata-description-format-default";
}
