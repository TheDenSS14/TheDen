// SPDX-FileCopyrightText: 2019 Injazz
// SPDX-FileCopyrightText: 2019 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2020 chairbender
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Javier Guardia Fernández
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2021 komunre
// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Jezithyr
// SPDX-FileCopyrightText: 2023 Psychpsyo
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 Aexxie
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Shared.Guidebook;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Explosion.Components
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class OnUseTimerTriggerComponent : Component
    {
        [DataField] public float Delay = 1f;

        /// <summary>
        ///     If not null, a user can use verbs to configure the delay to one of these options.
        /// </summary>
        [DataField] public List<float>? DelayOptions = null;

        /// <summary>
        ///     If not null, this timer will periodically play this sound while active.
        /// </summary>
        [DataField] public SoundSpecifier? BeepSound;

        /// <summary>
        ///     Time before beeping starts. Defaults to a single beep interval. If set to zero, will emit a beep immediately after use.
        /// </summary>
        [DataField] public float? InitialBeepDelay;

        [DataField] public float BeepInterval = 1;

        /// <summary>
        ///     Whether the timer should instead be activated through a verb in the right-click menu
        /// </summary>
        [DataField] public bool UseVerbInstead = false;

        /// <summary>
        ///     Should timer be started when it was stuck to another entity.
        ///     Used for C4 charges and similar behaviour.
        /// </summary>
        [DataField] public bool StartOnStick;

        /// <summary>
        ///     Allows changing the start-on-stick quality.
        /// </summary>
        [DataField("canToggleStartOnStick")] public bool AllowToggleStartOnStick;

        /// <summary>
        ///     Whether you can examine the item to see its timer or not.
        /// </summary>
        [DataField] public bool Examinable = true;

        #region GuidebookData

        [GuidebookData]
        public float? ShortestDelayOption => DelayOptions?.Min();

        [GuidebookData]
        public float? LongestDelayOption => DelayOptions?.Max();

        #endregion GuidebookData
    }
}
