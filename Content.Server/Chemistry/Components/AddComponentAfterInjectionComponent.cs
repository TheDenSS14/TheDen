// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.Components;

[RegisterComponent]
public sealed partial class AddComponentAfterInjectionComponent : Component
{
    [DataField("addComponents")]
    public ComponentRegistry ComponentsToAdd = new();
}
