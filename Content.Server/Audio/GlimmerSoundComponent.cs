// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Psionics.Glimmer;
using Content.Shared.Audio;
using Content.Shared.Psionics.Glimmer;
using Robust.Shared.Audio;
using Robust.Shared.ComponentTrees;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Serialization;

namespace Content.Server.Audio
{
    [RegisterComponent]
    [Access(typeof(SharedAmbientSoundSystem), typeof(GlimmerReactiveSystem))]
    public sealed partial class GlimmerSoundComponent : Component
    {
        [DataField("glimmerTier", required: true), ViewVariables(VVAccess.ReadWrite)] // only for map editing
        public Dictionary<string, SoundSpecifier> Sound { get; set; } = new();

        public bool GetSound(GlimmerTier glimmerTier, out SoundSpecifier? spec)
        {
            return Sound.TryGetValue(glimmerTier.ToString(), out spec);
        }
    }
}
