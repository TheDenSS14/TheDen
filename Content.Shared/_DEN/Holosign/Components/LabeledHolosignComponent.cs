// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Holosign.Systems;
using Robust.Shared.GameStates;


namespace Content.Shared._DEN.Holosign.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedLabelableHolosignProjectorSystem))]
public sealed partial class LabeledHolosignComponent : Component
{
    [AutoNetworkedField]
    public string Description;

    [AutoNetworkedField]
    public bool IsNSFW;
}
