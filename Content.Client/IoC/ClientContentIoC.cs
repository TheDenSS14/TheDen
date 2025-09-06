// SPDX-FileCopyrightText: 2019 DamianX
// SPDX-FileCopyrightText: 2019 moneyl
// SPDX-FileCopyrightText: 2020 Exp
// SPDX-FileCopyrightText: 2020 PrPleGoo
// SPDX-FileCopyrightText: 2020 chairbender
// SPDX-FileCopyrightText: 2021 Moony
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 ShadowCommander
// SPDX-FileCopyrightText: 2022 20kdc
// SPDX-FileCopyrightText: 2022 Acruid
// SPDX-FileCopyrightText: 2022 Jezithyr
// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 Miro Kavaliou
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 DrSmugleaf
// SPDX-FileCopyrightText: 2024 Leon Friedrich
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 Vasilis
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Administration.Managers;
using Content.Client.Changelog;
using Content.Client.Chat.Managers;
using Content.Client.Clickable;
using Content.Client._DV.NanoChat;
using Content.Client._Floof.Consent;
using Content.Client.DiscordAuth;
using Content.Client.JoinQueue;
using Content.Client.DebugMon;
using Content.Client.Eui;
using Content.Client.Fullscreen;
using Content.Client.GameTicking.Managers;
using Content.Client.GhostKick;
using Content.Client.Guidebook;
using Content.Client.Launcher;
using Content.Client.Mapping;
using Content.Client.Parallax.Managers;
using Content.Client.Players.PlayTimeTracking;
using Content.Client.Replay;
using Content.Client.Screenshot;
using Content.Client.Stylesheets;
using Content.Client.Viewport;
using Content.Client.Voting;
using Content.Shared.Administration.Logs;
using Content.Client.Lobby;
using Content.Client.Players.RateLimiting;
using Content.Shared.Administration.Managers;
using Content.Shared.Chat;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Players.RateLimiting;
namespace Content.Client.IoC
{
    internal static class ClientContentIoC
    {
        public static void Register()
        {
            var collection = IoCManager.Instance!;

            collection.Register<IParallaxManager, ParallaxManager>();
            collection.Register<IChatManager, ChatManager>();
            collection.Register<ISharedChatManager, ChatManager>();
            collection.Register<IClientConsentManager, ClientConsentManager>();
            collection.Register<IClientPreferencesManager, ClientPreferencesManager>();
            collection.Register<IStylesheetManager, StylesheetManager>();
            collection.Register<IScreenshotHook, ScreenshotHook>();
            collection.Register<FullscreenHook, FullscreenHook>();
            collection.Register<IClickMapManager, ClickMapManager>();
            collection.Register<IClientAdminManager, ClientAdminManager>();
            collection.Register<ISharedAdminManager, ClientAdminManager>();
            collection.Register<EuiManager, EuiManager>();
            collection.Register<IVoteManager, VoteManager>();
            collection.Register<ChangelogManager, ChangelogManager>();
            collection.Register<ViewportManager, ViewportManager>();
            collection.Register<ISharedAdminLogManager, SharedAdminLogManager>();
            collection.Register<GhostKickManager>();
            collection.Register<ExtendedDisconnectInformationManager>();
            collection.Register<JobRequirementsManager>();
            collection.Register<DocumentParsingManager>();
            collection.Register<ContentReplayPlaybackManager>();
            collection.Register<ISharedPlaytimeManager, JobRequirementsManager>();
            collection.Register<JoinQueueManager>();
            collection.Register<DiscordAuthManager>();
            collection.Register<PlayerRateLimitManager>();
            collection.Register<SharedPlayerRateLimitManager, PlayerRateLimitManager>();
            collection.Register<NanoChatSystem>();
            collection.Register<MappingManager>();
            collection.Register<DebugMonitorManager>();
            collection.Register<TitleWindowManager>();
        }
    }
}
