// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;

namespace Content.Server.Psionics
{
    /// <summary>
    ///     A component for weapons intended to have special effects when wielded against Psionic Entities.
    /// </summary>
    [RegisterComponent]
    public sealed partial class AntiPsionicWeaponComponent : Component
    {

        [DataField(required: true)]
        public DamageModifierSet Modifiers = default!;

        [DataField]
        public float PsychicStaminaDamage = 30f;

        /// <summary>
        ///     How long (in seconds) should this weapon temporarily disable powers
        /// </summary>
        [DataField]
        public float DisableDuration = 10f;

        /// <summary>
        ///     The chances of this weapon temporarily disabling psionic powers
        /// </summary>
        [DataField]
        public float DisableChance = 0.3f;

        /// <summary>
        ///     The condition to be inflicted on a Psionic entity
        /// </summary>
        [DataField]
        public string DisableStatus = "PsionicsDisabled";

        /// <summary>
        ///     Whether or not the user of this weapon risks Punishment by the gods if they dare use it on non-Psionic Entities
        /// </summary
        [DataField]
        public bool Punish = true;

        /// <summary>
        ///     The odds of divine punishment per non-Psionic Entity attacked
        /// </summary>
        [DataField]
        public float PunishChances = 0.5f;

        /// <summary>
        ///     How much Shock damage to take when Punish(ed) by the gods for using this weapon
        /// </summary>
        [DataField]
        public int PunishSelfDamage = 20;

        /// <summary>
        ///     How long (in seconds) should the user be stunned when punished by the gods
        /// </summary>
        [DataField]
        public float PunishStunDuration = 5f;
    }
}
