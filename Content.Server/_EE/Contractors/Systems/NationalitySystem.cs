// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server._DEN.Customization.Systems;
using Content.Server.Players.PlayTimeTracking;
using Content.Shared._EE.Contractors.Prototypes;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;

namespace Content.Server._EE.Contractors.Systems;

public sealed class NationalitySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly ISerializationManager _serialization = default!;
    [Dependency] private readonly CharacterRequirementsSystem _characterRequirements = default!;
    [Dependency] private readonly PlayTimeTrackingManager _playTimeTracking = default!;
    [Dependency] private readonly IConfigurationManager _configuration = default!;
    [Dependency] private readonly IComponentFactory _componentFactory = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);
    }

    // When the player is spawned in, add the nationality components selected during character creation
    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent args) =>
        ApplyNationality(args.Mob, args.JobId, args.Profile, args.Player);

    /// <summary>
    ///     Adds the nationality selected by a player to an entity.
    /// </summary>
    public void ApplyNationality(EntityUid uid,
        ProtoId<JobPrototype>? jobId,
        HumanoidCharacterProfile profile,
        ICommonSession player)
    {
        if (jobId == null || !_prototype.TryIndex(jobId, out var jobPrototypeToUse))
            return;

        var nationality = profile.Nationality != string.Empty
            ? profile.Nationality
            : SharedHumanoidAppearanceSystem.DefaultNationality;

        if (!_prototype.TryIndex<NationalityPrototype>(nationality, out var nationalityPrototype))
        {
            DebugTools.Assert($"Nationality '{nationality}' not found!");
            return;
        }

        var context = _characterRequirements.GetProfileContext(player, profile)
            .WithSelectedJob(jobPrototypeToUse)
            .WithPrototype(nationalityPrototype);

        if (!_characterRequirements.CheckRequirementsValid(nationalityPrototype.Requirements,
            context,
            EntityManager,
            _prototype,
            _configuration))
            return;

        AddNationality(uid, nationalityPrototype);
    }

    /// <summary>
    ///     Adds a single Nationality Prototype to an Entity.
    /// </summary>
    public void AddNationality(EntityUid uid, NationalityPrototype nationalityPrototype)
    {
        foreach (var function in nationalityPrototype.Functions)
            function.OnPlayerSpawn(uid, _componentFactory, EntityManager, _serialization);
    }
}
