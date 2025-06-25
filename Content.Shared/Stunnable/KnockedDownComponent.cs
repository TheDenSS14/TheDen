// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tirochora <leotabletdb@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Standing;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Stunnable;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedStunSystem))]
public sealed partial class KnockedDownComponent : Component
{
    [DataField("helpInterval"), AutoNetworkedField]
    public float HelpInterval = 1f;

    [DataField("helpAttemptSound")]
    public SoundSpecifier StunAttemptSound = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg");

    [DataField]
    public DropHeldItemsBehavior DropHeldItemsBehavior = DropHeldItemsBehavior.DropIfStanding;

    [DataField]
    public bool FollowUp = false;

    [ViewVariables, AutoNetworkedField]
    public float HelpTimer = 0f;
}
