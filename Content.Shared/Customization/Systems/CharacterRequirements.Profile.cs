// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 Raikyr0
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Prototypes;
using Content.Shared.Traits;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Physics;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;


/// <summary>
///     Requires the profile to be within an age range
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterAgeRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public int Min;

    [DataField]
    public int Max = Int32.MaxValue;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var localeString = "character-age-requirement-range";
        if (Max == Int32.MaxValue || Min <= 0)
            localeString = Max == Int32.MaxValue
                ? "character-age-requirement-minimum-only"
                : "character-age-requirement-maximum-only";

        return Loc.GetString(
            localeString,
            ("inverted", Inverted),
            ("min", Min),
            ("max", Max));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile is not null
            && context.Profile.Age >= Min
            && context.Profile.Age <= Max;
    }
}

/// <summary>
///     Requires the profile to be a certain gender
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterGenderRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public Gender Gender;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var genderKey = Gender.ToString().ToLower();
        return Loc.GetString(
            "character-gender-requirement",
            ("inverted", Inverted),
            ("gender", Loc.GetString($"humanoid-profile-editor-pronouns-{genderKey}-text")));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile is not null && context.Profile.Gender == Gender;
    }
}

/// <summary>
///     Requires the profile to be a certain sex
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterSexRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public Sex Sex;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var sexKey = Sex.ToString().ToLower();
        return Loc.GetString(
            "character-sex-requirement",
            ("inverted", Inverted),
            ("sex", Loc.GetString($"humanoid-profile-editor-sex-{sexKey}-text")));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile is not null && context.Profile.Sex == Sex;
    }
}

/// <summary>
///     Requires the profile to be a certain species
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterSpeciesRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<SpeciesPrototype>> Species;

    private const string RequirementColor = "green";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var speciesNames = Species.Select(s => Loc.GetString(prototypeManager.Index(s).Name));
        var colorTags = $"[/color], [color={RequirementColor}]";

        return Loc.GetString(
            "character-species-requirement",
            ("inverted", Inverted),
            ("species", $"[color={RequirementColor}]{string.Join(colorTags, speciesNames)}[/color]"));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile is not null
            && Species.Contains(context.Profile.Species);
    }
}

/// <summary>
///     Requires the profile to be within a certain height range
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterHeightRequirement : CharacterRequirement
{
    /// <summary>
    ///     The minimum height of the profile in centimeters
    /// </summary>
    [DataField]
    public float Min = int.MinValue;

    /// <summary>
    ///     The maximum height of the profile in centimeters
    /// </summary>
    [DataField]
    public float Max = int.MaxValue;

    private const string RequirementColor = "yellow";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString(
            "character-height-requirement",
            ("inverted", Inverted),
            ("color", RequirementColor),
            ("min", Min),
            ("max", Max));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (context.Profile is null)
            return false;

        var species = prototypeManager.Index<SpeciesPrototype>(context.Profile.Species);
        var height = context.Profile.Height * species.AverageHeight;
        return height >= Min && height <= Max;
    }
}

/// <summary>
///     Requires the profile to be within a certain width range
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterWidthRequirement : CharacterRequirement
{
    /// <summary>
    ///     The minimum width of the profile in centimeters
    /// </summary>
    [DataField]
    public float Min = int.MinValue;

    /// <summary>
    ///     The maximum width of the profile in centimeters
    /// </summary>
    [DataField]
    public float Max = int.MaxValue;

    private const string RequirementColor = "yellow";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString(
            "character-width-requirement",
            ("inverted", Inverted),
            ("color", RequirementColor),
            ("min", Min),
            ("max", Max));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (context.Profile is null)
            return false;

        var species = prototypeManager.Index<SpeciesPrototype>(context.Profile.Species);
        var width = context.Profile.Width * species.AverageWidth;
        return width >= Min && width <= Max;
    }
}

/// <summary>
///     Requires the profile to be within a certain weight range
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterWeightRequirement : CharacterRequirement
{
    /// <summary>
    ///     Minimum weight of the profile in kilograms
    /// </summary>
    [DataField]
    public float Min = int.MinValue;

    /// <summary>
    ///     Maximum weight of the profile in kilograms
    /// </summary>
    [DataField]
    public float Max = int.MaxValue;

    private const string RequirementColor = "green";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString(
            "character-weight-requirement",
            ("inverted", Inverted),
            ("color", RequirementColor),
            ("min", Min),
            ("max", Max));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (context.Profile is null)
            return false;

        var species = prototypeManager.Index<SpeciesPrototype>(context.Profile.Species);
        var dummyProto = prototypeManager.Index(species.Prototype);
        var compFactory = IoCManager.Resolve<IComponentFactory>();

        if (!dummyProto.TryGetComponent<FixturesComponent>(out var fixture, compFactory))
            return false;

        // TODO: Apparently this doesn't affect anything? wtf

        // Area of the circular fixture
        // (pi * radius^2, where radius is multiplied by the average scale of the profile)
        var baseRadius = fixture.Fixtures["fix1"].Shape.Radius;
        var averageSize = (context.Profile.Width + context.Profile.Height) / 2;
        var area = MathF.PI * MathF.Pow(baseRadius * averageSize, 2);

        // Mass = area * density
        var density = fixture.Fixtures["fix1"].Density;
        var weight = MathF.Round(area * density);

        return weight >= Min && weight <= Max;
    }
}

/// <summary>
///     Requires the profile to have one of the specified traits
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterTraitRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<TraitPrototype>> Traits;

    private const string RequirementColor = "lightblue";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var traitNames = Traits.Select(t => Loc.GetString($"trait-name-{t}"));
        var traitList = string.Join($"[/color], [color={RequirementColor}]", traitNames);

        return Loc.GetString(
            "character-trait-requirement",
            ("inverted", Inverted),
            ("traits", $"[color={RequirementColor}]{traitList}[/color]"));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile != null
            && Traits.Any(t => context.Profile.TraitPreferences.Contains(t.ToString()));
    }
}

/// <summary>
///     Requires the profile to have one of the specified loadouts
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterLoadoutRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<LoadoutPrototype>> Loadouts;

    private const string RequirementColor = "lightblue";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var loadoutNames = Loadouts.Select(l => Loc.GetString($"loadout-name-{l}"));
        var loadoutList = string.Join($"[/color], [color={RequirementColor}]", loadoutNames);

        return Loc.GetString(
            "character-loadout-requirement",
            ("inverted", Inverted),
            ("loadouts", $"[color={RequirementColor}]{loadoutList}[/color]"));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile != null
            && Loadouts.Any(l => context.Profile.LoadoutPreferences
                .Select(l => l.LoadoutName).Contains(l.ToString()));
    }
}

/// <summary>
///     Requires the profile to not have any more than X of the specified traits, loadouts, etc, in a group
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterItemGroupRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public ProtoId<CharacterItemGroupPrototype> Group;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null
            && context.Prototype is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var group = prototypeManager.Index(Group);
        return Loc.GetString(
            "character-item-group-requirement",
            ("inverted", Inverted),
            ("group", Loc.GetString($"character-item-group-{Group}")),
            ("max", group.MaxItems));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (context.Profile == null || context.Prototype == null)
            return false;

        var group = prototypeManager.Index(Group);
        var items = group.Items
            .Where(item => item.TryGetValue(context.Profile, prototypeManager, out _))
            .Select(item => item.ID)
            .ToList();
        var count = items.Count;

        // this is a little silly...
        var isLoadout = prototypeManager.TryIndex<LoadoutPrototype>(context.Prototype.ID, out var loadoutPrototype);
        var isTrait = prototypeManager.TryIndex<TraitPrototype>(context.Prototype.ID, out var traitPrototype);

        if (items.Contains(context.Prototype.ID))
        {
            if (isLoadout)
                count -= loadoutPrototype!.Slots;
            else if (isTrait)
                count -= traitPrototype!.ItemGroupSlots;
            else
                count--;
        }

        return count < group.MaxItems;
    }
}
