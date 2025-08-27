// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server._DEN.Customization.Systems;
using Content.Server.Players.PlayTimeTracking;
using Content.Shared._EE.Contractors.Prototypes;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid;
using Content.Shared.Players;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;

namespace Content.Server._EE.Contractors.Systems;

public sealed class LifepathSystem : EntitySystem
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

    // When the player is spawned in, add the Lifepath components selected during character creation
    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent args) =>
        ApplyLifepath(args.Mob, args.JobId, args.Profile, args.Player);

    /// <summary>
    ///     Adds the Lifepath selected by a player to an entity.
    /// </summary>
    public void ApplyLifepath(EntityUid uid,
        ProtoId<JobPrototype>? jobId,
        HumanoidCharacterProfile profile,
        ICommonSession player)
    {
        if (jobId == null || !_prototype.TryIndex(jobId, out var jobPrototypeToUse))
            return;

        var lifepath = profile.Lifepath != string.Empty
            ? profile.Lifepath
            : SharedHumanoidAppearanceSystem.DefaultLifepath;

        if (!_prototype.TryIndex<LifepathPrototype>(lifepath, out var lifepathPrototype))
        {
            DebugTools.Assert($"Lifepath '{lifepath}' not found!");
            return;
        }

        var context = _characterRequirements.GetProfileContext(player, profile)
            .WithSelectedJob(jobPrototypeToUse)
            .WithPrototype(lifepathPrototype);

        if (!_characterRequirements.CheckRequirementsValid(lifepathPrototype.Requirements,
            context,
            EntityManager,
            _prototype,
            _configuration))
            return;

        AddLifepath(uid, lifepathPrototype);
    }

    /// <summary>
    ///     Adds a single Lifepath Prototype to an Entity.
    /// </summary>
    public void AddLifepath(EntityUid uid, LifepathPrototype lifepathPrototype)
    {
        foreach (var function in lifepathPrototype.Functions)
            function.OnPlayerSpawn(uid, _componentFactory, EntityManager, _serialization);
    }
}
