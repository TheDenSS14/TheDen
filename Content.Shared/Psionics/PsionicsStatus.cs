// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

/// <summary>
/// EVERYTHING HERE IS A MODIFIED VERSION OF CRIMINAL RECORDS
/// </summary>

namespace Content.Shared.Psionics;

/// <summary>
/// Status used in Psionics Records.
///
/// None - the default value
/// Suspected - the person is suspected of having psionics
/// Registered - the person is a registered psionics user
/// Abusing - the person has been caught abusing their psionics
/// </summary>
public enum PsionicsStatus : byte
{
    None,
    Suspected,
    Registered,
    Abusing
}
