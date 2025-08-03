// SPDX-FileCopyrightText: 2024 Spatison
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Content.Shared.Alert;
using Content.Shared.Inventory.VirtualItem;


namespace Content.Shared.OfferItem;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
[Access(typeof(SharedOfferItemSystem))]
public sealed partial class OfferItemComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool IsInOfferMode;

    [DataField, AutoNetworkedField]
    public bool IsInReceiveMode;

    [DataField, AutoNetworkedField]
    public string? Hand;

    [DataField, AutoNetworkedField]
    public EntityUid? Item;

    [DataField, AutoNetworkedField]
    public EntityUid? Target;

    [DataField]
    public float MaxOfferDistance = 2f;

    [DataField]
    public ProtoId<AlertPrototype> OfferAlert = "Offer";

    // Floofstation section
    public EntityUid GetRealEntity(EntityManager entityManager) =>
        entityManager.GetComponentOrNull<VirtualItemComponent>(Item)?.BlockingEntity ?? Item ?? EntityUid.Invalid;
    // Floofstation section end
}
