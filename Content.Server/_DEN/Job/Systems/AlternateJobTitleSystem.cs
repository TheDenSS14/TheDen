// SPDX-FileCopyrightText: 2025 Dirius77
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server._DEN.Job.Components;
using Content.Server.Access.Systems;
using Content.Server.Mind;
using Content.Server.Roles.Jobs;
using Content.Shared._DEN.Job;
using Content.Shared.Clothing.Loadouts.Systems;


namespace Content.Server._DEN.Job.Systems;


/// <summary>
/// This handles spawning with an alternate job title.
/// </summary>
public sealed partial class AlternateJobTitleSystem : EntitySystem
{
    [Dependency] private readonly IdCardSystem _idCard = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly JobSystem _job = default!;
    [Dependency] private readonly MindSystem _mind = default!;

    private ISawmill _sawmill = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _log.GetSawmill("alternate-job-title");

        SubscribeLocalEvent<GetJobNameEvent>(OnGetJobName);
        SubscribeLocalEvent<PlayerLoadoutAppliedEvent>(OnPlayerLoadoutApplied);
    }

    private void OnGetJobName(ref GetJobNameEvent args)
    {
        if (!args.Profile.JobTitles.TryGetValue(args.Job.ID, out var selectedTitle)
            || string.IsNullOrWhiteSpace(selectedTitle))
            return;

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
        SetupJobName(ev);
    }

    private void SetupJobName(PlayerLoadoutAppliedEvent ev)
    {
        var playerEntity = ev.Mob;
        var hasId = _idCard.TryFindIdCard(playerEntity, out var idCard);
        var mindId = _mind.GetMind(playerEntity);

        if (!mindId.HasValue)
            return;

        var hasJob = _job.MindTryGetJob(mindId.Value, out var job);

        if (!hasId || !hasJob || job == null)
            return;

        var jobNameEvent = new GetJobNameEvent(job, ev.Profile);
        RaiseLocalEvent(ref jobNameEvent);

        // PresetIdCardSystem runs after us, mark the card so it doesn't change it.
        if (jobNameEvent.Handled)
            EnsureComp<AlternateJobTitleComponent>(idCard, out var _);
        // idk why station record isn't updated when job title is, even with a bool. insane.
        _idCard.TryChangeJobTitle(idCard, jobNameEvent.JobName, idCard.Comp);
        _idCard.UpdateStationRecord(idCard, newJobTitle: jobNameEvent.JobName);
    }
}
