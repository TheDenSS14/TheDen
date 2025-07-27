using Content.Shared.Roles;


namespace Content.Shared._DEN.Job;

[ByRefEvent]
public sealed record GetJobNameEvent
{
    public bool Handled { get; set; }
    public JobPrototype Job { get; set; }
    public string JobName { get; set; } = string.Empty;

    public GetJobNameEvent(JobPrototype job)
    {
        Job = job;
        JobName = job.LocalizedName;
    }
}
