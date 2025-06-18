// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Server._Lavaland.Mobs.Hierophant.Components;

[RegisterComponent]
public sealed partial class HierophantFieldGeneratorComponent : Component
{
    [ViewVariables]
    public bool Enabled;

    [ViewVariables]
    public List<EntityUid> Walls = new();

    [DataField]
    public int Radius;

    [DataField]
    public EntProtoId HierophantPrototype = "LavalandBossHierophant";

    [DataField]
    public EntProtoId WallPrototype = "WallHierophantArenaTemporary";

    [DataField]
    public EntityUid? ConnectedHierophant;
}
