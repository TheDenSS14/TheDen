// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Cocoon
{
    [RegisterComponent]
    public sealed partial class CocoonComponent : Component
    {
        public string? OldAccent;

        public EntityUid? Victim;

        [DataField("damagePassthrough")]
        public float DamagePassthrough = 0.5f;

    }
}
