// SPDX-FileCopyrightText: 2024 SleepyScarecrow <136123749+SleepyScarecrow@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

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
}

