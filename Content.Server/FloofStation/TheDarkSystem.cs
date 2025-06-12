using Content.Server.GameTicking.Events;
using Content.Server.Shuttles.Components;
using Content.Shared.Shuttles.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Map;

namespace Content.Server.FloofStation;

public sealed class TheDarkSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly MapLoaderSystem _loader = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundStartingEvent>(SetupTheDark);
    }

    private void SetupTheDark(RoundStartingEvent ev)
    {
        var mapId = _mapManager.CreateMap();
        _mapManager.AddUninitializedMap(mapId);

        if (!_loader.TryLoad(mapId, "/Maps/_Floof/hideout.yml", out var uids))
            return;

        foreach (var id in uids)
        {
            EnsureComp<PreventPilotComponent>(id);
        }

        _mapManager.DoMapInitialize(mapId);
    }
}
