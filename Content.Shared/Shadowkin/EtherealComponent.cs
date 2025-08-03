// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NPC.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;


namespace Content.Shared.Shadowkin;

[RegisterComponent, NetworkedComponent]
public sealed partial class EtherealComponent : Component
{
    /// <summary>
    ///     Does the Ent, Dark lights around it?
    /// </summary>
    [DataField]
    public bool Darken = false;

    /// <summary>
    ///     Range of the Darken Effect.
    /// </summary>
    [DataField]
    public float DarkenRange = 5;

    /// <summary>
    ///     Darken Effect Rate.
    /// </summary>
    [DataField]
    public float DarkenRate = 0.084f;

    /// Can this be stunned by ethereal stun objects?
    [DataField]
    public bool CanBeStunned = true;

    public List<EntityUid> DarkenedLights = new();

    public float DarkenAccumulator;

    public int OldMobMask;

    public int OldMobLayer;

    public List<ProtoId<NpcFactionPrototype>> SuppressedFactions = new();
    public bool HasDoorBumpTag;
}
