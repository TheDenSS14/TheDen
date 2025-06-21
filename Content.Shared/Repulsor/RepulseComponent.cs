// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Repulsor;

[RegisterComponent]
public sealed partial class RepulseComponent : Component
{
    [DataField]
    public float ForceMultiplier = 13000;

    [DataField]
    public TimeSpan KnockdownDuration = TimeSpan.FromSeconds(3);

    [DataField]
    public TimeSpan StunDuration = TimeSpan.FromSeconds(3);
}

public sealed class BeforeRepulseEvent(EntityUid target) : CancellableEntityEventArgs
{
    public EntityUid Target = target;
}
