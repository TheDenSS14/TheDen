// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Preferences;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._EE.Contractors.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class PassportComponent : Component
{
    public bool IsClosed;

    [ViewVariables]
    public HumanoidCharacterProfile OwnerProfile;
}
