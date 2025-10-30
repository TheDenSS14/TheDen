// SPDX-FileCopyrightText: 2025 Dirius77
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Preferences;
using Content.Shared.Roles;


namespace Content.Shared._DEN.Job;

[ByRefEvent]
public sealed record GetJobNameEvent
{
    public bool Handled { get; set; }
    public JobPrototype Job { get; set; }
    public HumanoidCharacterProfile Profile { get; set; }
    public string JobName { get; set; } = string.Empty;

    public GetJobNameEvent(JobPrototype job, HumanoidCharacterProfile profile)
    {
        Job = job;
        JobName = job.LocalizedName;
        Profile = profile;
    }
}
