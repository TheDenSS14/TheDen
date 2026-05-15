// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Holosign.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.Holosign.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Access(typeof(SharedLabelableHolosignProjectorSystem))]
public sealed partial class LabelableHolosignProjectorComponent : Component
{
    /// <summary>
    /// The entity to spawn with this projector.
    /// </summary>
    [DataField(required: true), Access(Other = AccessPermissions.ReadWriteExecute)]
    public List<EntProtoId> SignProtos;

    [DataField, AutoNetworkedField] public EntProtoId? SelectedSignProto;

    [DataField]
    public EntityWhitelist SignWhitelist;

    [DataField]
    public bool UsesCharges = false;

    [ViewVariables(VVAccess.ReadWrite), Access(Other = AccessPermissions.ReadWriteExecute)]
    [DataField, AutoNetworkedField]
    public string BarrierDescription = string.Empty;

    /// <summary>
    /// The maximum length of a description that can be attached to a barrier.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public int MaxDescriptionChars = 512;
    
    [DataField, AutoNetworkedField]
    public bool IsNsfw;
}