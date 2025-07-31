// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 csqrb <56765288+CaptainSqrBeard@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aikakakah <145503852+Aikakakah@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Lyndomen <49795619+Lyndomen@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Humanoid.Markings;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Preferences; // DeltaV
using Robust.Shared.Enums;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;
using Content.Shared.Preferences; //DeltaV, used for Metempsychosis, Fugitive, and Paradox Anomaly

namespace Content.Shared.Humanoid;

[NetworkedComponent, RegisterComponent, AutoGenerateComponentState(true)]
public sealed partial class HumanoidAppearanceComponent : Component
{
    public MarkingSet ClientOldMarkings = new();

    [DataField, AutoNetworkedField]
    public MarkingSet MarkingSet = new();

    [DataField]
    public Dictionary<HumanoidVisualLayers, HumanoidSpeciesSpriteLayer> BaseLayers = new();

    [DataField, AutoNetworkedField]
    public HashSet<HumanoidVisualLayers> PermanentlyHidden = new();

    // Couldn't these be somewhere else?

    // TheDen - Add Voice
    [DataField, AutoNetworkedField]
    public Sex PreferredVoice;

    [DataField, AutoNetworkedField]
    public Gender Gender;

    [DataField, AutoNetworkedField]
    public string? DisplayPronouns;

    [DataField, AutoNetworkedField]
    public string? StationAiName;

    [DataField, AutoNetworkedField]
    public string? CyborgName;

    [DataField, AutoNetworkedField]
    public int Age = 18;

    [DataField, AutoNetworkedField]
    public string CustomSpecieName = "";

    /// <summary>
    ///     Any custom base layers this humanoid might have. See:
    ///     limb transplants (potentially), robotic arms, etc.
    ///     Stored on the server, this is merged in the client into
    ///     all layer settings.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<HumanoidVisualLayers, CustomBaseLayerInfo> CustomBaseLayers = new();

    /// <summary>
    ///     Current species. Dictates things like base body sprites,
    ///     base humanoid to spawn, etc.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public ProtoId<SpeciesPrototype> Species { get; set; }

    /// <summary>
    ///     The initial profile and base layers to apply to this humanoid.
    /// </summary>
    [DataField]
    public ProtoId<HumanoidProfilePrototype>? Initial { get; private set; }

    /// <summary>
    ///     Skin color of this humanoid.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Color SkinColor { get; set; } = Color.FromHex("#C0967F");

    /// <summary>
    ///     Visual layers currently hidden. This will affect the base sprite
    ///     on this humanoid layer, and any markings that sit above it.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<HumanoidVisualLayers> HiddenLayers = new();

    /// <summary>
    /// The specific markings that are hidden, whether or not the layer is hidden.
    /// This is so we can just turn off a single marking, or part of a single marking.
    /// (cus underwear, its for underwear, so you can take off your bra and still have your shirt on)
    /// FLOOF ADD
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<string> HiddenMarkings = new();

    [DataField, AutoNetworkedField]
    public Sex Sex = Sex.Male;

    [DataField, AutoNetworkedField]
    public Color EyeColor = Color.Brown;

    /// <summary>
    ///     Hair color of this humanoid. Used to avoid looping through all markings
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Color? CachedHairColor;

    /// <summary>
    ///     Facial Hair color of this humanoid. Used to avoid looping through all markings
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Color? CachedFacialHairColor;

    /// <summary>
    ///     Which layers of this humanoid that should be hidden on equipping a corresponding item..
    /// </summary>
    [DataField]
    public HashSet<HumanoidVisualLayers> HideLayersOnEquip = [HumanoidVisualLayers.Hair];

    /// <summary>
    /// DeltaV - let paradox anomaly be cloned
    /// </summary>
    [ViewVariables]
    public HumanoidCharacterProfile? LastProfileLoaded;

    /// <summary>
    ///     The height of this humanoid.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Height = 1f;

    /// <summary>
    ///     The width of this humanoid.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Width = 1f;
}

[DataDefinition]
[Serializable, NetSerializable]
public readonly partial struct CustomBaseLayerInfo
{
    public CustomBaseLayerInfo(string? id, Color? color = null)
    {
        DebugTools.Assert(id == null || IoCManager.Resolve<IPrototypeManager>().HasIndex<HumanoidSpeciesSpriteLayer>(id));
        Id = id;
        Color = color;
    }

    /// <summary>
    ///     ID of this custom base layer. Must be a <see cref="HumanoidSpeciesSpriteLayer"/>.
    /// </summary>
    [DataField]
    public ProtoId<HumanoidSpeciesSpriteLayer>? Id { get; init; }

    /// <summary>
    ///     Color of this custom base layer. Null implies skin colour if the corresponding <see cref="HumanoidSpeciesSpriteLayer"/> is set to match skin.
    /// </summary>
    [DataField]
    public Color? Color { get; init; }
}
