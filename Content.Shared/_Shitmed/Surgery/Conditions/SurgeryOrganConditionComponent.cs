// SPDX-FileCopyrightText: 2024 gluesniffler
// SPDX-FileCopyrightText: 2025 Spatison
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Body.Organ;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryOrganConditionComponent : Component
{
    [DataField]
    public ComponentRegistry? Organ;

    [DataField]
    public bool Inverse;

    [DataField]
    public bool Reattaching;

    [DataField(required: true)]
    public string SlotId = string.Empty;
}
