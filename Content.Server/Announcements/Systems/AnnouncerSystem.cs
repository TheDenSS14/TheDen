// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chat.Systems;
using Content.Shared.GameTicking;
using Content.Shared.Announcements.Prototypes;
using Content.Shared.Announcements.Systems;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Server.Announcements.Systems;

public sealed partial class AnnouncerSystem : SharedAnnouncerSystem
{
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly ChatSystem _chat = default!;

    /// <summary>
    ///     The currently selected announcer
    /// </summary>
    [Access(typeof(AnnouncerSystem))]
    public AnnouncerPrototype Announcer { get; set; } = default!;


    public override void Initialize()
    {
        base.Initialize();
        NewAnnouncer();

        _config.OnValueChanged(CCVars.Announcer, _ => NewAnnouncer());

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestarting);
    }
}
