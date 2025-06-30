// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Lavaland.Damage;

[RegisterComponent]
public sealed partial class HierophantClubItemComponent : Component
{
    [DataField]
    public EntProtoId CreateCrossActionId = "ActionHierophantSpawnCross";

    [DataField]
    public EntProtoId PlaceMarkerActionId = "ActionHierophantPlaceMarker";

    [DataField]
    public EntProtoId TeleportToMarkerActionId = "ActionHierophantTeleport";

    [DataField]
    public EntityUid? CreateCrossActionEntity;

    [DataField]
    public EntityUid? PlaceMarkerActionEntity;

    [DataField]
    public EntityUid? TeleportToMarkerActionEntity;

    [DataField]
    public EntityUid? TeleportMarker;

    [DataField]
    public float CrossRange = 5f;

    [DataField]
    public SoundSpecifier DamageSound = new SoundPathSpecifier("/Audio/_Lavaland/Mobs/Bosses/hiero_blast.ogg");

    [DataField]
    public SoundSpecifier TeleportSound = new SoundPathSpecifier("/Audio/Magic/blink.ogg");
}
