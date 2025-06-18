// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.WhiteDream.BloodCult.UI;

[NetSerializable, Serializable]
public enum BloodRitesUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class BloodRitesUiState(Dictionary<EntProtoId, float> crafts, FixedPoint2 storedBlood)
    : BoundUserInterfaceState
{
    public Dictionary<EntProtoId, float> Crafts = crafts;
    public FixedPoint2 StoredBlood = storedBlood;
}

[Serializable, NetSerializable]
public sealed class BloodRitesMessage(EntProtoId selectedProto) : BoundUserInterfaceMessage
{
    public EntProtoId SelectedProto = selectedProto;
}
