// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2025 Milon
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Customization.Systems;
using Robust.Shared.GameStates;
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

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ExtendDescriptionComponent : Component
{
    /// <summary>
    ///     The list of all descriptions to add to an entity when examined at close range.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<DescriptionExtension> DescriptionList = new();
}
