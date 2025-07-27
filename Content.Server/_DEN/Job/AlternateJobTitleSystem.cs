using System.Linq;
using Content.Server.Access.Systems;
using Content.Server.Mind;
using Content.Server.Roles.Jobs;
using Content.Shared._DEN.Job;
using Content.Shared.Access.Systems;
using Content.Shared.Clothing.Loadouts.Systems;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Job;


/// <summary>
/// This handles spawning with an alternate job title.
/// </summary>
public sealed class AlternateJobTitleSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IdCardSystem _idCard = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly JobSystem _job = default!;
    [Dependency] private readonly MindSystem _mind = default!;

    private ProtoId<AlternateJobTitlePrototype> DebugJobPrototype => "Captain";
    private ISawmill _sawmill = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        _sawmill = _log.GetSawmill("alternate-job-title");

        SubscribeLocalEvent<GetJobNameEvent>(OnGetJobName);
        SubscribeLocalEvent<PlayerLoadoutAppliedEvent>(OnPlayerLoadoutApplied);
    }

    private void OnGetJobName(ref GetJobNameEvent args)
    {
        if (args.Handled
            || args.Job.ID != DebugJobPrototype.Id
            || !_prototype.TryIndex(DebugJobPrototype, out var jobTitlePrototype))
        {
            _sawmill.Info($"Job ID is required job: {args.Job.ID == DebugJobPrototype.Id}");
            _sawmill.Info($"Handled: {args.Handled}");
            return;
        }

        var selectedTitle = jobTitlePrototype.Titles.First();
        var nameExists = Loc.TryGetString(selectedTitle, out var name);

        if (!nameExists || name == null)
        {
            _sawmill.Warning($"Alternate job title `{selectedTitle}` is invalid.");
            return;
        }

        args.JobName = name;
        args.Handled = true;
    }

    private void OnPlayerLoadoutApplied(PlayerLoadoutAppliedEvent ev)
    {
        SetupJobName(ev.Mob);
    }

    private void SetupJobName(EntityUid playerEntity)
    {
        var hasId = _idCard.TryFindIdCard(playerEntity, out var idCard);
        var mindId = _mind.GetMind(playerEntity);

        if (!mindId.HasValue)
            return;

        var hasJob = _job.MindTryGetJob(mindId.Value, out var job);

        if (!hasId || !hasJob || job == null)
            return;

        var jobNameEvent = new GetJobNameEvent(job);
        RaiseLocalEvent(ref jobNameEvent);

        // idk why station record isn't updated when job title is, even with a bool. insane.
        _idCard.TryChangeJobTitle(idCard, jobNameEvent.JobName, idCard.Comp);
        _idCard.UpdateStationRecord(idCard, newJobTitle: jobNameEvent.JobName);
    }
}
