// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Lavaland.Aggression;

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class AggressorComponent : Component
{
    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadOnly)] public HashSet<EntityUid> Aggressives = new();
}
