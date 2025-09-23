// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Scribbles0 <91828755+Scribbles0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Speech.Muting
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class MutedComponent : Component
    {
        // IMP start
        /// <summary>
        /// Whether the affected entity can speak.
        /// </summary>
        [DataField(serverOnly: true)]
        public bool MutedSpeech = true;

        /// <summary>
        /// Whether the affected entity emotes will have sound.
        /// </summary>
        [DataField(serverOnly: true)]
        public bool MutedEmotes = true;

        /// <summary>
        /// Whether the affected entity will be able to use the scream action.
        /// </summary>
        [DataField(serverOnly: true)]
        public bool MutedScream = true;
        // IMP end
    }
}
