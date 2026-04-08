// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Floof.Consent;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Consent;


/// <summary>
/// This is used for networking consents to client-sided systems.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ConsentComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public List<ProtoId<ConsentTogglePrototype>> NotDefaultConsents = new();
}
