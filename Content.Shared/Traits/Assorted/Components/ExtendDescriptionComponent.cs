// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Customization.Systems;
using Robust.Shared.Serialization;

namespace Content.Shared.Traits.Assorted.Components;

[Serializable, NetSerializable, DataDefinition]
public sealed partial class DescriptionExtension
{
    [DataField]
    public List<CharacterRequirement>? Requirements;

    [DataField]
    public string Description = "";

    [DataField]
    public string RequirementsNotMetDescription = "";

    [DataField]
    public int FontSize = 12;

    [DataField]
    public string Color = "#ffffff";

    [DataField]
    public bool RequireDetailRange = true;
}

[RegisterComponent]
public sealed partial class ExtendDescriptionComponent : Component
{
    /// <summary>
    ///     The list of all descriptions to add to an entity when examined at close range.
    /// </summary>
    [DataField]
    public List<DescriptionExtension> DescriptionList = new();
}
