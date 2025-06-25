// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Psionics;
using Robust.Shared.Serialization;

/// <summary>
/// EVERYTHING HERE IS A MODIFIED VERSION OF CRIMINAL RECORDS
/// </summary>

namespace Content.Shared.PsionicsRecords;

/// <summary>
/// Psionics record for a crewmember.
/// Can be viewed and edited in a psionics records console by epistemics.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public sealed record PsionicsRecord
{
    /// <summary>
    /// Status of the person (None, Suspect, Registered, Abusing).
    /// </summary>
    [DataField]
    public PsionicsStatus Status = PsionicsStatus.None;

    /// <summary>
    /// When Status is Anything but none, the reason for it.
    /// Should never be set otherwise.
    /// </summary>
    [DataField]
    public string? Reason;
}
