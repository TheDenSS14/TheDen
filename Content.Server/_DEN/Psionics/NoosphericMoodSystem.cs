using Content.Shared._DEN.Traits.EntitySystems;
using Content.Shared.Mood;
using Content.Shared.Psionics.Glimmer;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using NoosphericMoodComponent = Content.Shared._DEN.Traits.Components.NoosphericMoodComponent;


namespace Content.Server._DEN.Psionics;


public sealed class NoosphericMoodSystem : SharedNoosphericMoodSystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly GlimmerSystem _glimmer = default!;

    private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(10);
    private TimeSpan _nextUpdate;

    public override void Initialize()
    {
        _nextUpdate = _gameTiming.CurTime;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_nextUpdate > _gameTiming.CurTime)
            return;

        var query = EntityQueryEnumerator<NoosphericMoodComponent>();
        while (query.MoveNext(out var entity, out var comp))
        {
            ApplyNoosphereMood((entity, comp), _glimmer.GlimmerOutput);
        }

        _nextUpdate = _gameTiming.CurTime + _updateInterval;
    }

    private void ApplyNoosphereMood(Entity<NoosphericMoodComponent> entity, double glimmer)
    {
        ProtoId<MoodEffectPrototype>? correctMood = null;
        foreach (var threshold in entity.Comp.GlimmerThresholds)
        {
            if (threshold.Value <= glimmer)
            {
                correctMood = threshold.Key;
                break;
            }
        }

        if (correctMood is not { } mood)
            return;

        Log.Debug("Setting mood for: " + mood.Id);

        var ev = new MoodEffectEvent(mood);
        RaiseLocalEvent(entity.Owner, ev);
    }
}
