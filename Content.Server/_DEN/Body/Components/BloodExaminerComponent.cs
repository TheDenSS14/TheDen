// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Body.Systems;

namespace Content.Server._DEN.Body.Systems;

/// <summary>
///     This entity can see the bloodstream reagents of other species.
///     Note that this does not detect all chemicals in the bloodstream - just whatever their
///     actual BloodstreamComponent blood is.
/// </summary>
[RegisterComponent, Access(typeof(BloodstreamSystem))]
public sealed partial class BloodExaminerComponent : Component
{
    [DataField]
    public LocId ExamineText = "blood-examiner-component-examine";

    [DataField]
    public LocId BloodSuffix = "blood-examiner-component-blood-suffix";
}
