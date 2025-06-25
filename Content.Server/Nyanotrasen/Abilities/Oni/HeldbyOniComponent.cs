// SPDX-FileCopyrightText: 2023 Adrian16199 <144424013+Adrian16199@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Abilities.Oni
{
    [RegisterComponent]
    public sealed partial class HeldByOniComponent : Component
    {
        public EntityUid Holder = default!;
    }
}
