using Content.Server.Research.Systems;
using Content.Shared._DEN.Research;
using Content.Shared.Research.Components;
using Content.Shared.Research.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Research.Systems;


/// <summary>
/// This handles unlocking specific research on map initialization of the component.
/// </summary>
public sealed class UnlockResearchSystem : EntitySystem
{
    [Dependency] private readonly ResearchSystem _research = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    private HashSet<ProtoId<TechnologyPrototype>> _technologies = new();

    /// <inheritdoc/>
    public override void Initialize()
    {
        _prototype.PrototypesReloaded += _ => GenerateTechnologies();

        SubscribeLocalEvent<UnlockResearchComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<UnlockResearchComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.StartingPoints > 0)
            _research.ModifyServerPoints(ent, ent.Comp.StartingPoints);

        if (ent.Comp.UnlockAll)
            UnlockAllResearch(ent);

        if (ent.Comp.Technologies != null && ent.Comp.Technologies.Count > 0)
            UnlockSpecificResearch(ent, ent.Comp.Technologies);

        if (TryComp<TechnologyDatabaseComponent>(ent, out var database))
            _research.UpdateTechnologyCards(ent, database);
    }

    /// <summary>
    /// Cache the technologies instead of enumerating prototypes every time.
    /// </summary>
    private void GenerateTechnologies()
    {
        var technologies = _prototype.EnumeratePrototypes<TechnologyPrototype>();
        _technologies.Clear();

        foreach (var technology in technologies)
            _technologies.Add(technology);
    }

    private void UnlockAllResearch(Entity<UnlockResearchComponent> ent)
    {
        if (!TryComp<TechnologyDatabaseComponent>(ent, out var technologyDatabase))
            return;

        foreach (var technology in _technologies)
            _research.AddTechnology(ent, technology, technologyDatabase);
    }

    private void UnlockSpecificResearch(Entity<UnlockResearchComponent> ent,
        HashSet<ProtoId<TechnologyPrototype>> technologies)
    {
        if (!TryComp<TechnologyDatabaseComponent>(ent, out var technologyDatabase))
            return;

        foreach (var technology in technologies)
            _research.AddTechnology(ent, technology, technologyDatabase);
    }
}
