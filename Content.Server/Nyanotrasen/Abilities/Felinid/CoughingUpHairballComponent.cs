// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Abilities.Felinid;

[RegisterComponent]
public sealed partial class CoughingUpHairballComponent : Component
{
    [DataField("accumulator")]
    public float Accumulator = 0f;

    [DataField("coughUpTime")]
    public TimeSpan CoughUpTime = TimeSpan.FromSeconds(2.15); // length of hairball.ogg
}
