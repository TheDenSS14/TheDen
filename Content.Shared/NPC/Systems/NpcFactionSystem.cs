// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 Paulo Artur Pinheiro Viana Villaça <112904295+AlgumCorrupto@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 stellar-novas <stellar_novas@riseup.net>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NPC.Components;
using Content.Shared.NPC.Events;
using Content.Shared.NPC.Prototypes;
using Robust.Shared.Prototypes;
using System.Collections.Frozen;
using System.Linq;

namespace Content.Shared.NPC.Systems;

/// <summary>
///     Outlines faction relationships with each other.
///     part of psionics rework was making this a partial class. Should've already been handled upstream, based on the linter.
/// </summary>
public sealed partial class NpcFactionSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    /// <summary>
    /// To avoid prototype mutability we store an intermediary data class that gets used instead.
    /// </summary>
    private FrozenDictionary<string, FactionData> _factions = FrozenDictionary<string, FactionData>.Empty;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NpcFactionMemberComponent, ComponentStartup>(OnFactionStartup);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(OnProtoReload);

        InitializeException();
        InitializeCore();
        InitializeItems();
        RefreshFactions();
    }

    private void OnProtoReload(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<NpcFactionPrototype>())
            RefreshFactions();
    }

    private void OnFactionStartup(Entity<NpcFactionMemberComponent> ent, ref ComponentStartup args)
    {
        RefreshFactions(ent);
    }

    /// <summary>
    /// Refreshes the cached factions for this component.
    /// </summary>
    private void RefreshFactions(Entity<NpcFactionMemberComponent> ent)
    {
        ent.Comp.FriendlyFactions.Clear();
        ent.Comp.HostileFactions.Clear();

        foreach (var faction in ent.Comp.Factions)
        {
            // YAML Linter already yells about this, don't need to log an error here
            if (!_factions.TryGetValue(faction, out var factionData))
                continue;

            ent.Comp.FriendlyFactions.UnionWith(factionData.Friendly);
            ent.Comp.HostileFactions.UnionWith(factionData.Hostile);
        }
    }

    /// <summary>
    /// Returns whether an entity is a member of a faction.
    /// </summary>
    public bool IsMember(Entity<NpcFactionMemberComponent?> ent, string faction)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return false;

        return ent.Comp.Factions.Contains(faction);
    }

    /// <summary>
    /// Returns whether an entity is a member of any listed faction.
    /// If the list is empty this returns false.
    /// </summary>
    public bool IsMemberOfAny(Entity<NpcFactionMemberComponent?> ent, IEnumerable<ProtoId<NpcFactionPrototype>> factions)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return false;

        foreach (var faction in factions)
        {
            if (ent.Comp.Factions.Contains(faction))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Adds this entity to the particular faction.
    /// </summary>
    public void AddFaction(Entity<NpcFactionMemberComponent?> ent, string faction, bool dirty = true)
    {
        if (!_proto.HasIndex<NpcFactionPrototype>(faction))
        {
            Log.Error($"Unable to find faction {faction}");
            return;
        }

        ent.Comp ??= EnsureComp<NpcFactionMemberComponent>(ent);
        if (!ent.Comp.Factions.Add(faction))
            return;

        RaiseLocalEvent(ent.Owner, new NpcFactionAddedEvent(faction));

        if (dirty)
            RefreshFactions((ent, ent.Comp));
    }

    /// <summary>
    /// Adds this entity to the particular factions.
    /// </summary>
    public void AddFactions(Entity<NpcFactionMemberComponent?> ent, HashSet<ProtoId<NpcFactionPrototype>> factions, bool dirty = true)
    {
        ent.Comp ??= EnsureComp<NpcFactionMemberComponent>(ent);

        foreach (var faction in factions)
        {
            if (!_proto.HasIndex(faction))
            {
                Log.Error($"Unable to find faction {faction}");
                continue;
            }

            RaiseLocalEvent(ent.Owner, new NpcFactionAddedEvent(faction));

            ent.Comp.Factions.Add(faction);
        }

        if (dirty)
            RefreshFactions((ent, ent.Comp));
    }

    /// <summary>
    /// Removes this entity from the particular faction.
    /// </summary>
    public void RemoveFaction(Entity<NpcFactionMemberComponent?> ent, string faction, bool dirty = true)
    {
        if (!_proto.HasIndex<NpcFactionPrototype>(faction))
        {
            Log.Error($"Unable to find faction {faction}");
            return;
        }

        if (!Resolve(ent, ref ent.Comp, false))
            return;

        if (!ent.Comp.Factions.Remove(faction))
            return;

        RaiseLocalEvent(ent.Owner, new NpcFactionRemovedEvent(faction));

        if (dirty)
            RefreshFactions((ent, ent.Comp));
    }

    /// <summary>
    /// Remove this entity from all factions.
    /// </summary>
    public void ClearFactions(Entity<NpcFactionMemberComponent?> ent, bool dirty = true)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.Factions.Clear();

        if (dirty)
            RefreshFactions((ent, ent.Comp));
    }

    public IEnumerable<EntityUid> GetNearbyHostiles(Entity<NpcFactionMemberComponent?, FactionExceptionComponent?> ent, float range)
    {
        if (!Resolve(ent, ref ent.Comp1, false))
            return Array.Empty<EntityUid>();

        var hostiles = GetNearbyFactions(ent, range, ent.Comp1.HostileFactions)
            // ignore mobs that have both hostile faction and the same faction,
            // otherwise having multiple factions is strictly negative
            .Where(target => !IsEntityFriendly((ent, ent.Comp1), target));
        if (!Resolve(ent, ref ent.Comp2, false))
            return hostiles;

        // ignore anything from enemy faction that we are explicitly friendly towards
        var faction = (ent.Owner, ent.Comp2);
        return hostiles
            .Union(GetHostiles(faction))
            .Where(target => !IsIgnored(faction, target));
    }

    public IEnumerable<EntityUid> GetNearbyFriendlies(Entity<NpcFactionMemberComponent?> ent, float range)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return Array.Empty<EntityUid>();

        return GetNearbyFactions(ent, range, ent.Comp.FriendlyFactions);
    }

    private IEnumerable<EntityUid> GetNearbyFactions(EntityUid entity, float range, HashSet<ProtoId<NpcFactionPrototype>> factions)
    {
        var xform = Transform(entity);
        foreach (var ent in _lookup.GetEntitiesInRange<NpcFactionMemberComponent>(_xform.GetMapCoordinates((entity, xform)), range))
        {
            if (ent.Owner == entity)
                continue;

            if (!factions.Overlaps(ent.Comp.Factions))
                continue;

            yield return ent.Owner;
        }
    }

    /// <remarks>
    /// 1-way and purely faction based, ignores faction exception.
    /// </remarks>
    public bool IsEntityFriendly(Entity<NpcFactionMemberComponent?> ent, Entity<NpcFactionMemberComponent?> other)
    {
        if (!Resolve(ent, ref ent.Comp, false) || !Resolve(other, ref other.Comp, false))
            return false;

        var intersect = ent.Comp.Factions.Intersect(other.Comp.Factions); // factions which have both ent and other as members
        foreach (var faction in intersect)
            if (_factions[faction].IsHostileToSelf)
                return false;

        return intersect.Count() > 0 || ent.Comp.FriendlyFactions.Overlaps(other.Comp.Factions);
    }

    public bool IsFactionFriendly(string target, string with)
    {
        return _factions[target].Friendly.Contains(with) && _factions[with].Friendly.Contains(target);
    }

    public bool IsFactionFriendly(string target, Entity<NpcFactionMemberComponent?> with)
    {
        if (!Resolve(with, ref with.Comp, false))
            return false;

        return with.Comp.Factions.All(x => IsFactionFriendly(target, x)) ||
               with.Comp.FriendlyFactions.Contains(target);
    }

    public bool IsFactionHostile(string target, string with)
    {
        return _factions[target].Hostile.Contains(with) && _factions[with].Hostile.Contains(target);
    }

    public bool IsFactionHostile(string target, Entity<NpcFactionMemberComponent?> with)
    {
        if (!Resolve(with, ref with.Comp, false))
            return false;

        return with.Comp.Factions.All(x => IsFactionHostile(target, x)) ||
               with.Comp.HostileFactions.Contains(target);
    }

    public bool IsFactionNeutral(string target, string with)
    {
        return !IsFactionFriendly(target, with) && !IsFactionHostile(target, with);
    }

    /// <summary>
    /// Makes the source faction friendly to the target faction, 1-way.
    /// </summary>
    public void MakeFriendly(string source, string target)
    {
        if (!_factions.TryGetValue(source, out var sourceFaction))
        {
            Log.Error($"Unable to find faction {source}");
            return;
        }

        if (!_factions.ContainsKey(target))
        {
            Log.Error($"Unable to find faction {target}");
            return;
        }

        sourceFaction.Friendly.Add(target);
        sourceFaction.Hostile.Remove(target);
        RefreshFactions();
    }

    /// <summary>
    /// Makes the source faction hostile to the target faction, 1-way.
    /// </summary>
    public void MakeHostile(string source, string target)
    {
        if (!_factions.TryGetValue(source, out var sourceFaction))
        {
            Log.Error($"Unable to find faction {source}");
            return;
        }

        if (!_factions.ContainsKey(target))
        {
            Log.Error($"Unable to find faction {target}");
            return;
        }

        sourceFaction.Friendly.Remove(target);
        sourceFaction.Hostile.Add(target);
        RefreshFactions();
    }

    private void RefreshFactions()
    {
        _factions = _proto.EnumeratePrototypes<NpcFactionPrototype>().ToFrozenDictionary(
            faction => faction.ID,
            faction => new FactionData
            {
                IsHostileToSelf = faction.Hostile.Contains(faction.ID),
                Friendly = faction.Friendly.ToHashSet(),
                Hostile = faction.Hostile.ToHashSet()
            });

        var query = AllEntityQuery<NpcFactionMemberComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            comp.FriendlyFactions.Clear();
            comp.HostileFactions.Clear();
            RefreshFactions((uid, comp));
        }
    }
}
