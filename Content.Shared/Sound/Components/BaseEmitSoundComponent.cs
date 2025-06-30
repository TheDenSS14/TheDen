// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;

namespace Content.Shared.Sound.Components
{
    /// <summary>
    /// Base sound emitter which defines most of the data fields.
    /// Accepts both single sounds and sound collections.
    /// </summary>
    public abstract partial class BaseEmitSoundComponent : Component
    {
        public static readonly AudioParams DefaultParams = AudioParams.Default.WithVolume(-2f);

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("sound", required: true)]
        public SoundSpecifier? Sound;
    }
}
