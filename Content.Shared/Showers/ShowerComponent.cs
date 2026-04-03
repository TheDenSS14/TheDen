// SPDX-FileCopyrightText: 2024 SleepyScarecrow
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 MajorMoth
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Showers
{
    /// <summary>
    /// showers that can be enabled
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ShowerComponent : Component
    {
        /// <summary>
        /// Toggles shower.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool ToggleShower;

        // DEN start

        /// <summary>
        /// What state should the shower be on map init?
        /// </summary>
        [DataField]
        public ShowerStartupState StartupState = ShowerStartupState.Random;
        // DEN end

        [DataField("enableShowerSound")]
        public SoundSpecifier EnableShowerSound = new SoundPathSpecifier("/Audio/Ambience/Objects/shower_enable.ogg");

        public EntityUid? PlayingStream;

        [DataField("loopingSound")]
        public SoundSpecifier LoopingSound = new SoundPathSpecifier("/Audio/Ambience/Objects/shower_running.ogg");

    }


    [Serializable, NetSerializable]
    public enum ShowerVisuals : byte
    {
        ShowerVisualState,
    }

    [Serializable, NetSerializable]
    public enum ShowerVisualState : byte
    {
        Off,
        On
    }

    // DEN start
    [Serializable, NetSerializable]
    public enum ShowerStartupState : byte
    {
        Off,
        On,
        Random
    }
    // DEN end
}

