// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Errant
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 Fansana
// SPDX-FileCopyrightText: 2025 Skubman
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Clothing.Systems;
using Content.Server._DV.ParadoxAnomaly.Components;
using Content.Server.DetailExaminable;
using Content.Server.GenericAntag;
using Content.Server.Ghost.Roles;
using Content.Server.Ghost.Roles.Components;
using Content.Server.Psionics;
using Content.Server.Spawners.Components;
using Content.Server.Station.Systems;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Diagnostics.CodeAnalysis;
using Content.Server._Floof.Consent;
using Content.Server.Roles.Jobs;
using Content.Server.StationEvents.Events;
using Content.Shared._Floof.Consent;
using Content.Shared.Traits.Assorted.Components;


namespace Content.Server._DV.ParadoxAnomaly.Systems;

/// <summary>
/// 90% of the work is done by exterminator since its a reskin.
/// All the logic here is spawning since thats tricky.
/// </summary>
public sealed class ParadoxAnomalySystem : EntitySystem
{
    [Dependency] private readonly ConsentSystem _consent = default!;
    [Dependency] private readonly GenericAntagSystem _genericAntag = default!;
    [Dependency] private readonly GhostRoleSystem _ghostRole = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly SharedHumanoidAppearanceSystem _humanoid = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly SharedRoleSystem _role = default!;
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = default!;
    [Dependency] private readonly LoadoutSystem _loadout = default!;
    [Dependency] private readonly JobSystem _jobSystem = default!;

    private const string ParadoxAnomalyExamine = "examine-paradox-anomaly-message";

    private ISawmill _sawmill = default!;
    private readonly ProtoId<ConsentTogglePrototype> _paradoxAnomalyConsent = "ParadoxClone";
    private readonly EntProtoId _paradoxAnomalySpawnerId = "SpawnPointGhostParadoxAnomaly";
    private readonly EntProtoId _paradoxAnomalyRule = "ParadoAnomaly";


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BeforeMidRoundAntagSpawnEvent>(OnAttemptMidRoundAntagSpawn);
        SubscribeLocalEvent<ParadoxAnomalySpawnerComponent, TakeGhostRoleEvent>(OnTakeGhostRole);

        _sawmill = _logManager.GetSawmill("paradox-anomaly");
    }

    private void OnAttemptMidRoundAntagSpawn(ref BeforeMidRoundAntagSpawnEvent ev)
    {
        var isParadoxAnomaly = ev.MidRoundAntagRule.Comp.Spawner == _paradoxAnomalySpawnerId;

        if (!isParadoxAnomaly)
            return; // code below will be decently intensive

        var canParadoxAnomalySpawn = CanParadoxAnomalySpawn();

        ev.Cancelled = isParadoxAnomaly && !canParadoxAnomalySpawn;

        if (ev.Cancelled)
            _sawmill.Warning("Paradox anomaly spawn cancelled because no valid candidates were found.");
    }

    private void OnTakeGhostRole(Entity<ParadoxAnomalySpawnerComponent> ent, ref TakeGhostRoleEvent args)
    {
        if (!TrySpawnParadoxAnomaly(ent.Comp.Rule, out var twin))
            return;

        var role = Comp<GhostRoleComponent>(ent);
        _ghostRole.GhostRoleInternalCreateMindAndTransfer(args.Player, ent, twin.Value, role);
        _ghostRole.UnregisterGhostRole((ent.Owner, role));

        args.TookRole = true;
        QueueDel(ent);
    }

    public bool CanParadoxAnomalySpawn()
    {
        // Get a list of potential candidates
        var candidates = new List<(EntityUid, EntityUid, SpeciesPrototype, HumanoidCharacterProfile)>();
        var query = EntityQueryEnumerator<MindContainerComponent, HumanoidAppearanceComponent>();

        while (query.MoveNext(out var uid, out var mindContainer, out var humanoid))
        {
            if (humanoid.LastProfileLoaded is not {} profile)
                continue;

            if (!_proto.TryIndex(humanoid.Species, out var species))
                continue;

            if (_mind.GetMind(uid, mindContainer) is not {} mindId)
                continue;

            if (!_jobSystem.MindTryGetJob(mindId, out var job))
                continue;

            if (_role.MindIsAntagonist(mindId))
                continue;

            if (!_consent.HasConsent(uid, _paradoxAnomalyConsent))
                continue;

            // TODO: when metempsychosis real skip whoever has Karma

            candidates.Add((uid, mindId, species, profile));
        }

        return candidates.Count != 0;
    }

    private bool TrySpawnParadoxAnomaly(string rule, [NotNullWhen(true)] out EntityUid? twin)
    {
        twin = null;

        // Get a list of potential candidates
        var candidates = new List<(EntityUid, EntityUid, SpeciesPrototype, HumanoidCharacterProfile)>();
        var query = EntityQueryEnumerator<MindContainerComponent, HumanoidAppearanceComponent>();
        while (query.MoveNext(out var uid, out var mindContainer, out var humanoid))
        {
            if (humanoid.LastProfileLoaded is not {} profile)
                continue;

            if (!_proto.TryIndex<SpeciesPrototype>(humanoid.Species, out var species))
                continue;

            if (_mind.GetMind(uid, mindContainer) is not {} mindId)
                continue;

            if (!_jobSystem.MindTryGetJob(mindId, out var job))
                continue;

            if (_role.MindIsAntagonist(mindId))
                continue;

            if (!_consent.HasConsent(uid, _paradoxAnomalyConsent))
                continue;

            // TODO: when metempsychosis real skip whoever has Karma

            candidates.Add((uid, mindId, species, profile));
        }

        twin = SpawnParadoxAnomaly(candidates, rule);
        return twin != null;
    }

    private EntityUid? SpawnParadoxAnomaly(List<(EntityUid, EntityUid, SpeciesPrototype, HumanoidCharacterProfile)> candidates, string rule)
    {
        // Select a candidate.
        if (candidates.Count == 0)
            return null;

        var (uid, mindId, species, profile) = _random.Pick(candidates);
        return SpawnParadoxAnomaly((uid, mindId, species, profile), rule);
    }

    private EntityUid? SpawnParadoxAnomaly(
        (EntityUid uid, EntityUid mindId, SpeciesPrototype species, HumanoidCharacterProfile profile) candidate,
        string rule
    )
    {
        var uid = candidate.uid;
        var mindId = candidate.mindId;
        var species = candidate.species;
        var profile = candidate.profile;

        if (!_jobSystem.MindTryGetJob(mindId, out var job))
            return null;

        // Find a suitable spawn point.
        var station = _station.GetOwningStation(uid);
        var latejoins = new List<EntityUid>();
        var query = EntityQueryEnumerator<SpawnPointComponent>();
        while (query.MoveNext(out var spawnUid, out var spawnPoint))
        {
            if (spawnPoint.SpawnType != SpawnPointType.LateJoin)
                continue;

            if (_station.GetOwningStation(spawnUid) == station)
                latejoins.Add(spawnUid);
        }

        if (latejoins.Count == 0)
            return null;

        // Spawn the twin.
        var destination = Transform(_random.Pick(latejoins)).Coordinates;
        var spawned = Spawn(species.Prototype, destination);

        // Set the kill target to the chosen player
        // _terminator.SetTarget(spawned, mindId);
        _genericAntag.MakeAntag(spawned, rule);

        //////////////////////////
        //    /!\ WARNING /!\   //
        // MAJOR SHITCODE BELOW //
        //    /!\ WARNING /!\   //
        //////////////////////////

        // Copy the details.
        _humanoid.LoadProfile(spawned, profile);
        _metaData.SetEntityName(spawned, Name(uid));

        var detailCopy = EnsureComp<DetailExaminableComponent>(spawned);
        var spawnedDescExtension = EnsureComp<ExtendDescriptionComponent>(spawned);

        if (TryComp<DetailExaminableComponent>(uid, out var detail))
        {
            detailCopy.Content = detail.Content;
        }

        if (TryComp<ExtendDescriptionComponent>(uid, out var descExtension))
        {
            spawnedDescExtension.DescriptionList = descExtension.DescriptionList;
        }

        var examineMessage = Loc.GetString(ParadoxAnomalyExamine, ("entity", spawned));
        var descriptionExt = new DescriptionExtension
        {
            RequireDetailRange = true,
            FontSize = 12,
            Description = examineMessage
        };

        spawnedDescExtension.DescriptionList.Add(descriptionExt);

        if (job.StartingGear != null && _proto.TryIndex<StartingGearPrototype>(job.StartingGear, out var gear))
        {
            _stationSpawning.EquipStartingGear(spawned, gear);
            _stationSpawning.EquipIdCard(spawned,
                profile.Name,
                job,
                station);
            _loadout.ApplyCharacterLoadout(spawned, job, profile, [], false); // TODO: find a way to get playtimes, player, and whitelisted
        }

        foreach (var special in job.Special)
        {
            special.AfterEquip(spawned);
        }

        // TODO: In a future PR, make it so that the Paradox Anomaly spawns with a completely 1:1 clone of the victim's entire PsionicComponent.
        if (HasComp<PsionicComponent>(uid))
            EnsureComp<PsionicComponent>(spawned);

        return spawned;
    }

    public bool TrySpawnUserParadoxAnomaly(EntityUid target, [NotNullWhen(true)] out EntityUid? spawned)
    {
        spawned = null;

        if (!TryComp<HumanoidAppearanceComponent>(target, out var humanoid)
            || !TryComp<MindContainerComponent>(target, out var mindContainer)
            || humanoid.LastProfileLoaded is not {} profile
            || !_proto.TryIndex(humanoid.Species, out var species)
            || _mind.GetMind(target, mindContainer) is not {} mindId
            || !_jobSystem.MindTryGetJob(mindId, out var job)
            || _role.MindIsAntagonist(mindId)
            || !_consent.HasConsent(target, _paradoxAnomalyConsent))
            return false;

        spawned = SpawnParadoxAnomaly((target, mindId, species, profile), _paradoxAnomalyRule);
        return spawned != null;
    }
}
