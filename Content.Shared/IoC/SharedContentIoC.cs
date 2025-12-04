// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 Flipp Syder
// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2024 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared._AS.Consent;
using Content.Shared._DEN.Hub;
using Content.Shared._DEN.ServerContent;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Localizations;
using Content.Shared.Tag;
using Content.Shared.Whitelist;

namespace Content.Shared.IoC;

public static class SharedContentIoC
{
    public static void Register()
    {
        IoCManager.Register<MarkingManager, MarkingManager>();
        IoCManager.Register<ContentLocalizationManager, ContentLocalizationManager>();
        IoCManager.Register<TagSystem>();
        IoCManager.Register<EntityWhitelistSystem>();
        IoCManager.Register<SharedConsentCardSystem>();
        IoCManager.Register<ServerSelectiveContentManager>(); // DEN
    }
}
