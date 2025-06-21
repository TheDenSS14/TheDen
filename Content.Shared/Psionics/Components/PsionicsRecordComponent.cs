// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

/// <summary>
/// EVERYTHING HERE IS A MODIFIED VERSION OF CRIMINAL RECORDS
/// </summary>

namespace Content.Shared.Psionics.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PsionicsRecordComponent : Component
{
    /// <summary>
    ///     The icon that should be displayed based on the psionics status of the entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<PsionicsIconPrototype> StatusIcon = "PsionicsIconStatus";
}
