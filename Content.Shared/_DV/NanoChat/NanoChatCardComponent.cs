// SPDX-FileCopyrightText: 2024 Milon
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 Skubman
// SPDX-FileCopyrightText: 2025 Tobias Berger
// SPDX-FileCopyrightText: 2025 Will-Oliver-Br
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.CartridgeLoader.Cartridges;
using Content.Shared.PDA;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._DV.NanoChat;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedNanoChatSystem))]
[AutoGenerateComponentPause, AutoGenerateComponentState]
public sealed partial class NanoChatCardComponent : Component
{
    /// <summary>
    ///     The number assigned to this card.
    /// </summary>
    [DataField, AutoNetworkedField]
    public uint? Number;

    /// <summary>
    ///     All chat recipients stored on this card.
    /// </summary>
    [DataField]
    public Dictionary<uint, NanoChatRecipient> Recipients = new();

    /// <summary>
    ///     All messages stored on this card, keyed by recipient number.
    /// </summary>
    [DataField]
    public Dictionary<uint, List<NanoChatMessage>> Messages = new();

    /// <summary>
    ///     The NanoChat numbers that should not give a notification, even when notifications are enabled.
    /// </summary>
    [DataField]
    public HashSet<uint> MutedChats = [];

    /// <summary>
    ///     The currently selected chat recipient number.
    /// </summary>
    [DataField]
    public uint? CurrentChat;

    /// <summary>
    ///     The maximum amount of recipients this card supports.
    /// </summary>
    [DataField]
    public int MaxRecipients = 50;

    /// <summary>
    ///     Last time a message was sent, for rate limiting.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan LastMessageTime; // TODO: actually use this, compare against actor and not the card

    /// <summary>
    ///     Whether to send notifications.
    /// </summary>
    [DataField]
    public bool NotificationsMuted;

    /// <summary>
    ///     The PDA that this card is currently inserted to.
    /// </summary>
    [DataField]
    public EntityUid? PdaUid = null;

    ///     Whether the card's number should be listed in NanoChat's lookup
    /// </summary>
    [DataField]
    public bool ListNumber = true;
}
