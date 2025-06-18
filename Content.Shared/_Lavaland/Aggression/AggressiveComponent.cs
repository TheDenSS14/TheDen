// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Lavaland.Aggression;

/// <summary>
///     Keeps track of whoever attacked our mob, so that it could prioritize or randomize targets.
/// </summary>
[RegisterComponent, AutoGenerateComponentState]
public sealed partial class AggressiveComponent : Component
{
    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadOnly)] public HashSet<EntityUid> Aggressors = new();

    [AutoNetworkedField]
    [DataField] public float ForgiveTime = 10f;

    [AutoNetworkedField]
    [DataField] public float ForgiveRange = 10f;
}
