// SPDX-FileCopyrightText: 2025 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;
using Robust.Shared.Audio;

// <summary>
//   Locks an item to only be used in melee by entities with a specific component.
// </summary>

namespace Content.Shared._EE.Items;

[RegisterComponent]
public sealed partial class RestrictedMeleeComponent : Component
{
    [DataField]
    public EntityWhitelist? Whitelist;

    [DataField]
    public TimeSpan KnockdownDuration = TimeSpan.FromSeconds(2);

    [DataField]
    public string FailText { get; set; } = "restricted-melee-component-attack-fail-too-large";

    [DataField]
    public bool DoKnockdown = true;

    [DataField]
    public bool ForceDrop = true;

    [DataField]
    public SoundSpecifier FallSound = new SoundPathSpecifier("/Audio/Effects/slip.ogg");
}
