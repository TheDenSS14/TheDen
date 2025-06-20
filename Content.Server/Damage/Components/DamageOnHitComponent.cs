// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Content.Shared._Shitmed.Targeting;


// Damages the entity by a set amount when it hits someone.
// Can be used to make melee items limited-use or make an entity deal self-damage with unarmed attacks.
namespace Content.Server.Damage.Components;

[RegisterComponent]
public sealed partial class DamageOnHitComponent : Component
{
    [DataField("ignoreResistances")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool IgnoreResistances = true;

    [DataField("damage", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier Damage = default!;

    // <summary>
    //   The body parts to deal damage to.
    //   When there is more than one listed element,
    //   randomly selects between one of the elements.
    // </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public List<TargetBodyPart>? TargetParts = null;
}
