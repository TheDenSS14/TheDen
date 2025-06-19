// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Goobstation.Weapons.AmmoSelector;
using Robust.Shared.Prototypes;

namespace Content.Shared.Changeling;

[RegisterComponent]
public sealed partial class ChangelingReagentStingComponent : Component
{
    [DataField(required: true)]
    public ProtoId<ReagentStingConfigurationPrototype> Configuration;

    [DataField]
    public ProtoId<SelectableAmmoPrototype>? DartGunAmmo;
}
