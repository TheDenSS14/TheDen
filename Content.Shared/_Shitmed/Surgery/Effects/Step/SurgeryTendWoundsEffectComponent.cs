// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
namespace Content.Shared._Shitmed.Medical.Surgery.Effects.Step;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SurgeryTendWoundsEffectComponent : Component
{
    [DataField, AutoNetworkedField]
    public string MainGroup = "Brute";

    [DataField, AutoNetworkedField]
    public bool IsAutoRepeatable = true;

    [DataField, AutoNetworkedField]
    public DamageSpecifier Damage = default!;

    [DataField, AutoNetworkedField]
    public float HealMultiplier = 0.07f;
}