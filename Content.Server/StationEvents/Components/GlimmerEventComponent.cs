// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Psionics.Glimmer;

[RegisterComponent]
public sealed partial class GlimmerEventComponent : Component
{
    /// <summary>
    ///     Minimum glimmer value for event to be eligible. (Should be 100 at lowest.)
    /// </summary>
    [DataField("minimumGlimmer")]
    public int MinimumGlimmer = 100;

    /// <summary>
    ///     Maximum glimmer value for event to be eligible. (Remember 1000 is max glimmer period.)
    /// </summary>
    [DataField("maximumGlimmer")]
    public int MaximumGlimmer = 1000;

    /// <summary>
    ///     Will be used for _random.Next and subtracted from glimmer.
    ///     Lower bound.
    /// </summary>
    [DataField("glimmerBurnLower")]
    public int GlimmerBurnLower = 25;

    /// <summary>
    ///     Will be used for _random.Next and subtracted from glimmer.
    ///     Upper bound.
    /// </summary>
    [DataField("glimmerBurnUpper")]
    public int GlimmerBurnUpper = 70;

    [DataField("report")]
    public string SophicReport = "glimmer-event-report-generic";
}
