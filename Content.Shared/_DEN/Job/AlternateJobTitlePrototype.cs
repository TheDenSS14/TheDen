// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Job;


/// <summary>
/// This is a prototype for declaring alternate job titles.
/// </summary>
[Prototype()]
public sealed partial class AlternateJobTitlePrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public List<LocId> Titles { get; set; } = new();
}
