// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Psionics;

/// <summary>
/// ADAPTED FROM SECWATCH - DELTAV
/// </summary>

namespace Content.Server.CartridgeLoader.Cartridges;

[RegisterComponent, Access(typeof(PsiWatchCartridgeSystem))]
public sealed partial class PsiWatchCartridgeComponent : Component
{
    /// <summary>
    /// Only show people with these statuses.
    /// </summary>
    [DataField]
    public List<PsionicsStatus> Statuses = new()
    {
        PsionicsStatus.Abusing,
        PsionicsStatus.Registered,
        PsionicsStatus.Suspected
    };

    /// <summary>
    /// Station entity thats getting its records checked.
    /// </summary>
    [DataField]
    public EntityUid? Station;
}
