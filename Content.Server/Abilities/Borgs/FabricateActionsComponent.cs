// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Server.Abilities.Borgs;

[RegisterComponent]
public sealed partial class FabricateActionsComponent : Component
{
    /// <summary>
    ///     IDs of fabrication actions that the entity should receive with this component.
    /// </summary>
    [DataField]
    public List<EntProtoId> Actions = new();

    /// <summary>
    ///     Action entities added by this component.
    /// </summary>
    [DataField]
    public Dictionary<EntProtoId, EntityUid> ActionEntities = new();
}
