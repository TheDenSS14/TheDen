// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;

namespace Content.Shared.Abilities.Psionics
{
    /// <summary>
    /// Takes damage when dispelled.
    /// </summary>
    [RegisterComponent]
    public sealed partial class DamageOnDispelComponent : Component
    {
        [DataField(required: true)]
        public DamageSpecifier Damage = default!;

        [DataField]
        public float Variance = 0.5f;
    }
}
