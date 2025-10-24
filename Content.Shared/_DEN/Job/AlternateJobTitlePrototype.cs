// SPDX-FileCopyrightText: 2025 Dirius77
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Customization.Systems;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Job;


/// <summary>
/// This is a prototype for declaring alternate job titles.
/// </summary>
[Prototype]
public sealed partial class AlternateJobTitlePrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public ProtoId<JobPrototype> JobId { get; } = string.Empty;

    [DataField]
    public List<CharacterRequirement> Requirements { get; } = new();

    [DataField]
    public List<LocId> Titles { get; set; } = new();
}
