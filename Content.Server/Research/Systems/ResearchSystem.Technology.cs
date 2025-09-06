// SPDX-FileCopyrightText: 2022 Paul Ritter
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2025 Blitz
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 EctoplasmIsGood
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 pathetic meowmeow
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DEN.Research.Components;
using Content.Shared.Database;
using Content.Shared.Research.Components;
using Content.Shared.Research.Prototypes;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;


namespace Content.Server.Research.Systems;

public sealed partial class ResearchSystem
{
    /// <summary>
    /// Syncs the primary entity's database to that of the secondary entity's database.
    /// </summary>
    public void Sync(EntityUid primaryUid, EntityUid otherUid, TechnologyDatabaseComponent? primaryDb = null, TechnologyDatabaseComponent? otherDb = null)
    {
        if (!Resolve(primaryUid, ref primaryDb) || !Resolve(otherUid, ref otherDb))
            return;

        primaryDb.MainDiscipline = otherDb.MainDiscipline;
        primaryDb.CurrentTechnologyCards = otherDb.CurrentTechnologyCards;
        primaryDb.SupportedDisciplines = otherDb.SupportedDisciplines;
        primaryDb.UnlockedTechnologies = otherDb.UnlockedTechnologies;
        primaryDb.UnlockedRecipes = otherDb.UnlockedRecipes;

        Dirty(primaryUid, primaryDb);

        var ev = new TechnologyDatabaseModifiedEvent();
        RaiseLocalEvent(primaryUid, ref ev);
    }

    /// <summary>
    ///     If there's a research client component attached to the owner entity,
    ///     and the research client is connected to a research server, this method
    ///     syncs against the research server, and the server against the local database.
    /// </summary>
    /// <returns>Whether it could sync or not</returns>
    public void SyncClientWithServer(EntityUid uid, TechnologyDatabaseComponent? databaseComponent = null, ResearchClientComponent? clientComponent = null)
    {
        if (!Resolve(uid, ref databaseComponent, ref clientComponent, false))
            return;

        if (!TryComp<TechnologyDatabaseComponent>(clientComponent.Server, out var serverDatabase))
            return;

        Sync(uid, clientComponent.Server.Value, databaseComponent, serverDatabase);
    }

    /// <summary>
    /// Tries to add a technology to a database, checking if it is able to
    /// </summary>
    /// <returns>If the technology was successfully added</returns>
    public bool UnlockTechnology(EntityUid client,
        string prototypeid,
        EntityUid user,
        ResearchClientComponent? component = null,
        TechnologyDatabaseComponent? clientDatabase = null)
    {
        if (!PrototypeManager.TryIndex<TechnologyPrototype>(prototypeid, out var prototype))
            return false;

        return UnlockTechnology(client, prototype, user, component, clientDatabase);
    }

    /// <summary>
    /// Tries to add a technology to a database, checking if it is able to
    /// </summary>
    /// <returns>If the technology was successfully added</returns>
    public bool UnlockTechnology(EntityUid client,
        TechnologyPrototype prototype,
        EntityUid user,
        ResearchClientComponent? component = null,
        TechnologyDatabaseComponent? clientDatabase = null)
    {
        if (!Resolve(client, ref component, ref clientDatabase, false)
            || !TryGetClientServer(client, out var serverEnt, out _, component)
            || !CanServerUnlockTechnology(client, prototype, clientDatabase, component)
            || !PrototypeManager.TryIndex(prototype.Discipline, out var disciplinePrototype)
            || !TryComp<ResearchServerComponent>(serverEnt.Value, out var researchServer)
            || prototype.Cost * clientDatabase.SoftCapMultiplier > researchServer.Points)
            return false;

        var station = _station.GetOwningStation(client);
        var oldSoftCap = clientDatabase.SoftCapMultiplier;

        if (prototype.Tier >= disciplinePrototype.LockoutTier)
        {
            clientDatabase.SoftCapMultiplier *= prototype.SoftCapContribution;
            researchServer.CurrentSoftCapMultiplier *= prototype.SoftCapContribution;
        }

        if (station != null
            && Exists(station)
            && station != EntityUid.Invalid
            && TryComp<StationResearchRecordComponent>(station, out var record))
            record.SoftCapMultiplier = clientDatabase.SoftCapMultiplier;

        AddTechnology(serverEnt.Value, prototype);
        TrySetMainDiscipline(prototype, serverEnt.Value);
        ModifyServerPoints(serverEnt.Value, -(int) (prototype.Cost * oldSoftCap));
        UpdateTechnologyCards(serverEnt.Value);

        _adminLog.Add(
            LogType.Action,
            LogImpact.Medium,
            $"{ToPrettyString(user):player} unlocked {prototype.ID} (discipline: {prototype.Discipline}, tier: {prototype.Tier}) at {ToPrettyString(client)}, for server {ToPrettyString(serverEnt.Value)}.");
        return true;
    }

    /// <summary>
    ///     Adds a technology to the database without checking if it could be unlocked.
    /// </summary>
    [PublicAPI]
    public void AddTechnology(EntityUid uid, string technology, TechnologyDatabaseComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        if (!PrototypeManager.TryIndex<TechnologyPrototype>(technology, out var prototype))
            return;

        AddTechnology(uid, prototype, component);
    }

    /// <summary>
    ///     Adds a technology to the database without checking if it could be unlocked.
    /// </summary>
    public void AddTechnology(EntityUid uid, TechnologyPrototype technology, TechnologyDatabaseComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        //todo this needs to support some other stuff, too
        foreach (var generic in technology.GenericUnlocks)
        {
            if (generic.PurchaseEvent != null)
                RaiseLocalEvent(generic.PurchaseEvent);
        }

        component.UnlockedTechnologies.Add(technology.ID);
        var addedRecipes = new List<ProtoId<LatheRecipePrototype>>();

        foreach (var unlock in technology.RecipeUnlocks)
        {
            if (component.UnlockedRecipes.Contains(unlock))
                continue;

            component.UnlockedRecipes.Add(unlock);
            addedRecipes.Add(unlock);
        }

        Dirty(uid, component);

        var ev = new TechnologyDatabaseModifiedEvent(uid, technology.ID, addedRecipes); // Goobstation - Lathe message on recipes update
        RaiseLocalEvent(uid, ref ev);
    }

    /// <summary>
    ///     Returns whether a technology can be unlocked on this database,
    ///     taking parent technologies into account.
    /// </summary>
    /// <returns>Whether it could be unlocked or not</returns>
    public bool CanServerUnlockTechnology(EntityUid uid,
        TechnologyPrototype technology,
        TechnologyDatabaseComponent? database = null,
        ResearchClientComponent? client = null)
    {

        if (!Resolve(uid, ref client, ref database, false))
            return false;

        if (!TryGetClientServer(uid, out _, out var serverComp, client))
            return false;

        if (!IsTechnologyAvailable(database, technology))
            return false;

        if (technology.Cost * database.SoftCapMultiplier > serverComp.Points)
            return false;

        return true;
    }

    private void OnDatabaseRegistrationChanged(EntityUid uid, TechnologyDatabaseComponent component, ref ResearchRegistrationChangedEvent args)
    {
        if (args.Server != null)
            return;
        component.MainDiscipline = null;
        component.CurrentTechnologyCards = new List<string>();
        component.SupportedDisciplines = new List<string>();
        component.UnlockedTechnologies = new List<string>();
        component.UnlockedRecipes = new List<string>();
        Dirty(uid, component);
    }
}
