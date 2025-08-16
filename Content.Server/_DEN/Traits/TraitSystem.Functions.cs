// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Construction.Completions;
using Content.Shared._Shitmed.Body.Events;
using Content.Shared._Shitmed.BodyEffects;
using Content.Shared.Atmos.Components;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Body.Prototypes;
using Content.Shared.Body.Systems;
using Content.Shared.Database;
using Content.Shared.Traits;
using Content.Shared.Whitelist;
using JetBrains.Annotations;
//using JetBrains.FormatRipper.Elf;
using Robust.Server.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Server._DEN.Traits;

/// <summary>
/// A trait function that will add metabolizers to the entity's organs.
/// </summary>
[UsedImplicitly]
public sealed partial class TraitAddMetabolizer : TraitFunction
{
    /// <summary>
    /// What metabolizers to add to the entity's organs.
    /// </summary>
    [DataField(required: true)]
    public HashSet<ProtoId<MetabolizerTypePrototype>> Metabolizers = new();

    /// <summary>
    /// Conditions for the organs for them to receive the metabolizer.
    /// You can use this to only apply the metabolizer to the stomach, for instance.
    /// </summary>
    [DataField("organWhitelist")]
    public EntityWhitelist? Whitelist = null;

    /// <summary>
    /// Whether this trait should replace all metabolizers instead of simply adding them.
    /// </summary>
    [DataField]
    public bool Replace = false;

    public override void OnPlayerSpawn(EntityUid uid,
        IComponentFactory factory,
        IEntityManager entityManager,
        ISerializationManager serializationManager)
    {
        if (!entityManager.TryGetComponent<BodyComponent>(uid, out var body))
            return;

        var bodySystem = entityManager.System<SharedBodySystem>();
        var whitelistSystem = entityManager.System<EntityWhitelistSystem>();
        var logManager = IoCManager.Resolve<IAdminLogManager>();

        foreach (var (organId, _) in bodySystem.GetBodyOrgans(uid, body))
        {
            if (!entityManager.TryGetComponent<MetabolizerComponent>(organId, out var metabolizer)
                || whitelistSystem.IsWhitelistFail(Whitelist, organId))
                continue;

            if (Replace || metabolizer.MetabolizerTypes == null)
                metabolizer.MetabolizerTypes = Metabolizers;
            else
                metabolizer.MetabolizerTypes.UnionWith(Metabolizers);

            logManager.Add(LogType.Hunger,
                LogImpact.Low,
                $"Added metabolizers {string.Join(",", Metabolizers)} to {entityManager.ToPrettyString(organId)} in {entityManager.ToPrettyString(uid)} (REPLACE: {Replace}). New metabolizer list: {string.Join(",", metabolizer.MetabolizerTypes)}");
        }
    }
}

/// <summary>
/// A trait function that adds components to limbs. Currently used for 'hidden' cybernetics.
/// </summary>
[UsedImplicitly]
public sealed partial class TraitAddComponentToBodyPart : TraitFunction
{
    /// <summary>
    /// What components to add to the limb.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry Components = new();

    /// <summary>
    /// Which limbs the components should be added to.
    /// </summary>
    [DataField]
    public HashSet<BodyPartType> Parts = new();

    public override void OnPlayerSpawn(EntityUid uid,
        IComponentFactory factory,
        IEntityManager entityManager,
        ISerializationManager serializationManager)
    {
        var body = entityManager.System<BodySystem>();

        foreach (var bodyPart in body.GetBodyChildren(uid))
        {
            if (!Parts.Contains(bodyPart.Component.PartType))
                continue;

            bodyPart.Component.OnAdd = Components;
            bodyPart.Component.OnRemove = Components;
            entityManager.EnsureComponent<BodyPartEffectComponent>(bodyPart.Id);
            if (bodyPart.Component.Body != null)
                entityManager.System<BodyPartEffectSystem>().AddComponents(bodyPart.Component.Body.Value, bodyPart.Id, bodyPart.Component.OnAdd);
        }
    }
}
