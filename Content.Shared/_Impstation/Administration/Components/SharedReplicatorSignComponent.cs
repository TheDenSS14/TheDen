// SPDX-FileCopyrightText: 2025 beck
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared._Impstation.Administration.Components;

[NetworkedComponent]
public abstract partial class SharedReplicatorSignComponent : Component
{
    [DataField(required: true)]
    public ResPath SpritePath = new("_Impstation/Mobs/Replicator/replicator_sign.rsi");
}
