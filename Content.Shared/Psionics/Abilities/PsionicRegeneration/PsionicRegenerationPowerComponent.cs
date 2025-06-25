// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Fluffiest Floofers <9823203+FluffiestFloof@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Content.Shared.DoAfter;

namespace Content.Shared.Abilities.Psionics
{
    [RegisterComponent]
    public sealed partial class PsionicRegenerationPowerComponent : Component
    {
        [DataField]
        public DoAfterId? DoAfter;

        [DataField]
        public float EssenceAmount = 20;

        [DataField]
        public float UseDelay = 8f;
        [DataField]

        public SoundSpecifier SoundUse = new SoundPathSpecifier("/Audio/Psionics/heartbeat_fast.ogg");
    }
}

