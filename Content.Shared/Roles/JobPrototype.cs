// SPDX-FileCopyrightText: 2019 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 2020 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 scuffedjays <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 2021 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Morber <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 rolfero <45628623+rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Bakke <luringens@protonmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+electrojr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+slambamactionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 c4llv07e <38111072+c4llv07e@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 dootythefrooty <awhunter8@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Access;
using Content.Shared.Guidebook;
using Content.Shared.Customization.Systems;
using Content.Shared.Dataset;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Roles
{
    /// <summary>
    ///     Describes information for a single job on the station.
    /// </summary>
    [Prototype("job")]
    public sealed partial class JobPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("playTimeTracker", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<PlayTimeTrackerPrototype>))]
        public string PlayTimeTracker { get; private set; } = string.Empty;

        [DataField("supervisors")]
        public string Supervisors { get; private set; } = "nobody";

        /// <summary>
        ///     The name of this job as displayed to players.
        /// </summary>
        [DataField("name")]
        public string Name { get; private set; } = string.Empty;

        [ViewVariables(VVAccess.ReadOnly)]
        public string LocalizedName => Loc.GetString(Name);

        /// <summary>
        ///     The name of this job as displayed to players.
        /// </summary>
        [DataField("description")]
        public string? Description { get; private set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string? LocalizedDescription => Description is null ? null : Loc.GetString(Description);

        [DataField("requirements")]
        public List<CharacterRequirement>? Requirements;

        [DataField("joinNotifyCrew")]
        public bool JoinNotifyCrew { get; private set; } = false;

        [DataField("requireAdminNotify")]
        public bool RequireAdminNotify { get; private set; } = false;

        [DataField("setPreference")]
        public bool SetPreference { get; private set; } = true;

        /// <summary>
        ///     Whether this job should show in the ID Card Console.
        ///     If set to null, it will default to SetPreference's value.
        /// </summary>
        [DataField]
        public bool? OverrideConsoleVisibility { get; private set; } = null;

        [DataField("canBeAntag")]
        public bool CanBeAntag { get; private set; } = true;

        /// <summary>
        /// Nyano/DV: For e.g. prisoners, they'll never use their latejoin spawner.
        /// </summary>
        [DataField("alwaysUseSpawner")]
        public bool AlwaysUseSpawner { get; } = false;

        /// <summary>
        ///     Whether this job is a head.
        ///     The job system will try to pick heads before other jobs on the same priority level.
        /// </summary>
        [DataField("weight")]
        public int Weight { get; private set; }

        /// <summary>
        /// How to sort this job relative to other jobs in the UI.
        /// Jobs with a higher value with sort before jobs with a lower value.
        /// If not set, <see cref="Weight"/> is used as a fallback.
        /// </summary>
        [DataField]
        public int? DisplayWeight { get; private set; }

        public int RealDisplayWeight => DisplayWeight ?? Weight;

        /// <summary>
        ///     A numerical score for how much easier this job is for antagonists.
        ///     For traitors, reduces starting TC by this amount. Other gamemodes can use it for whatever they find fitting.
        /// </summary>
        [DataField("antagAdvantage")]
        public int AntagAdvantage = 0;

        [DataField("startingGear", customTypeSerializer: typeof(PrototypeIdSerializer<StartingGearPrototype>))]
        public string? StartingGear { get; private set; }

        /// <summary>
        ///     If this has a value, it will randomly set the entity name of the
        ///     entity upon spawn based on the dataset.
        /// </summary>
        [DataField]
        public ProtoId<LocalizedDatasetPrototype>? NameDataset;

        /// <summary>
        ///   A list of requirements that when satisfied, add or replace from the base starting gear.
        /// </summary>
        [DataField("conditionalStartingGear")]
        public List<ConditionalStartingGear>? ConditionalStartingGears { get; private set; }

        /// <summary>
        /// Use this to spawn in as a non-humanoid (borg, test subject, etc.)
        /// Starting gear will be ignored.
        /// If you want to just add special attributes to a humanoid, use AddComponentSpecial instead.
        /// </summary>
        [DataField("jobEntity", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? JobEntity = null;

        [DataField]
        public ProtoId<JobIconPrototype> Icon { get; private set; } = "JobIconUnknown";

        [DataField("special", serverOnly: true)]
        public JobSpecial[] Special { get; private set; } = Array.Empty<JobSpecial>();

        [DataField("afterLoadoutSpecial", serverOnly: true)]
        public JobSpecial[] AfterLoadoutSpecial { get; private set; } = [];

        [DataField("access")]
        public IReadOnlyCollection<ProtoId<AccessLevelPrototype>> Access { get; private set; } = Array.Empty<ProtoId<AccessLevelPrototype>>();

        [DataField("accessGroups")]
        public IReadOnlyCollection<ProtoId<AccessGroupPrototype>> AccessGroups { get; private set; } = Array.Empty<ProtoId<AccessGroupPrototype>>();

        [DataField("extendedAccess")]
        public IReadOnlyCollection<ProtoId<AccessLevelPrototype>> ExtendedAccess { get; private set; } = Array.Empty<ProtoId<AccessLevelPrototype>>();

        [DataField("extendedAccessGroups")]
        public IReadOnlyCollection<ProtoId<AccessGroupPrototype>> ExtendedAccessGroups { get; private set; } = Array.Empty<ProtoId<AccessGroupPrototype>>();

        [DataField]
        public bool Whitelisted;

        [DataField]
        public bool SpawnLoadout = true;

        [DataField]
        public bool ApplyTraits = true;

        [DataField]
        public bool CanBeAntagTarget = true; // Floofstation Edit

        /// <summary>
        /// Optional list of guides associated with this role. If the guides are opened, the first entry in this list
        /// will be used to select the currently selected guidebook.
        /// </summary>
        [DataField]
        public List<ProtoId<GuideEntryPrototype>>? Guides;

    }

    /// <summary>
    ///   Starting gear that will only be applied upon satisfying requirements.
    /// </summary>
    [DataDefinition]
    public sealed partial class ConditionalStartingGear
    {
        /// <summary>
        ///   The requirements to check.
        /// </summary>
        [DataField(required: true)]
        public List<CharacterRequirement> Requirements;

        /// <summary>
        ///   The starting gear to apply, replacing the equivalent slots.
        /// </summary>
        [DataField(required: true)]
        public ProtoId<StartingGearPrototype> Id { get; private set; }

    }

    /// <summary>
    /// Sorts <see cref="JobPrototype"/>s appropriately for display in the UI,
    /// respecting their <see cref="JobPrototype.Weight"/>.
    /// </summary>
    public sealed class JobUIComparer : IComparer<JobPrototype>
    {
        public static readonly JobUIComparer Instance = new();

        public int Compare(JobPrototype? x, JobPrototype? y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (ReferenceEquals(null, y))
                return 1;
            if (ReferenceEquals(null, x))
                return -1;

            var cmp = -x.RealDisplayWeight.CompareTo(y.RealDisplayWeight);
            if (cmp != 0)
                return cmp;
            return string.Compare(x.ID, y.ID, StringComparison.Ordinal);
        }
    }
}
