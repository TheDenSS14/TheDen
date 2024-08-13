// SPDX-FileCopyrightText: 2018 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2019 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2019 Silver <Silvertorch5@gmail.com>
// SPDX-FileCopyrightText: 2020 Exp <theexp111@gmail.com>
// SPDX-FileCopyrightText: 2020 Hugal31 <hugo.laloge@gmail.com>
// SPDX-FileCopyrightText: 2020 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2020 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 chairbender <kwhipke1@gmail.com>
// SPDX-FileCopyrightText: 2020 derek <xderek.luix@gmail.com>
// SPDX-FileCopyrightText: 2020 moneyl <8206401+Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Michael Phillips <1194692+MeltedPixel@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 08A <git@08a.re>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Miro Kavaliou <miraslauk@gmail.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vasilis The Pikachu <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pierson Arnold <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2024 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 XavierSomething <tylernguyen203@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 Blahaj-the-Shonk <ambbc2@gmail.com>
// SPDX-FileCopyrightText: 2025 poemota <142114334+poeMota@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Input;

namespace Content.Shared.Input
{
    [KeyFunctions]
    public static class ContentKeyFunctions
    {
        public static readonly BoundKeyFunction UseItemInHand = "ActivateItemInHand";
        public static readonly BoundKeyFunction AltUseItemInHand = "AltActivateItemInHand";
        public static readonly BoundKeyFunction ActivateItemInWorld = "ActivateItemInWorld";
        public static readonly BoundKeyFunction AltActivateItemInWorld = "AltActivateItemInWorld";
        public static readonly BoundKeyFunction Drop = "Drop";
        public static readonly BoundKeyFunction ExamineEntity = "ExamineEntity";
        public static readonly BoundKeyFunction FocusChat = "FocusChatInputWindow";
        public static readonly BoundKeyFunction FocusLocalChat = "FocusLocalChatWindow";
        public static readonly BoundKeyFunction FocusEmote = "FocusEmote";
        public static readonly BoundKeyFunction FocusWhisperChat = "FocusWhisperChatWindow";
        public static readonly BoundKeyFunction FocusRadio = "FocusRadioWindow";
        public static readonly BoundKeyFunction FocusLOOC = "FocusLOOCWindow";
        public static readonly BoundKeyFunction FocusOOC = "FocusOOCWindow";
        public static readonly BoundKeyFunction FocusAdminChat = "FocusAdminChatWindow";
        public static readonly BoundKeyFunction FocusDeadChat = "FocusDeadChatWindow";
        public static readonly BoundKeyFunction FocusConsoleChat = "FocusConsoleChatWindow";
        public static readonly BoundKeyFunction CycleChatChannelForward = "CycleChatChannelForward";
        public static readonly BoundKeyFunction CycleChatChannelBackward = "CycleChatChannelBackward";
        public static readonly BoundKeyFunction EscapeContext = "EscapeContext";
        public static readonly BoundKeyFunction OpenCharacterMenu = "OpenCharacterMenu";
        public static readonly BoundKeyFunction OpenEmotesMenu = "OpenEmotesMenu";
        public static readonly BoundKeyFunction OpenLanguageMenu = "OpenLanguageMenu";
        public static readonly BoundKeyFunction OpenCraftingMenu = "OpenCraftingMenu";
        public static readonly BoundKeyFunction OpenGuidebook = "OpenGuidebook";
        public static readonly BoundKeyFunction OpenInventoryMenu = "OpenInventoryMenu";
        public static readonly BoundKeyFunction SmartEquipBackpack = "SmartEquipBackpack";
        public static readonly BoundKeyFunction SmartEquipPocket1 = "SmartEquipPocket1";
        public static readonly BoundKeyFunction SmartEquipPocket2 = "SmartEquipPocket2";
        public static readonly BoundKeyFunction SmartEquipBelt = "SmartEquipBelt";
        public static readonly BoundKeyFunction SmartEquipSuitStorage = "SmartEquipSuitStorage";
        public static readonly BoundKeyFunction OpenBackpack = "OpenBackpack";
        public static readonly BoundKeyFunction OpenBelt = "OpenBelt";
        public static readonly BoundKeyFunction OpenAHelp = "OpenAHelp";
        public static readonly BoundKeyFunction SwapHands = "SwapHands";
        public static readonly BoundKeyFunction MoveStoredItem = "MoveStoredItem";
        public static readonly BoundKeyFunction RotateStoredItem = "RotateStoredItem";
        public static readonly BoundKeyFunction SaveItemLocation = "SaveItemLocation";
        public static readonly BoundKeyFunction ThrowItemInHand = "ThrowItemInHand";
        public static readonly BoundKeyFunction TryPullObject = "TryPullObject";
        public static readonly BoundKeyFunction MovePulledObject = "MovePulledObject";
        public static readonly BoundKeyFunction ReleasePulledObject = "ReleasePulledObject";
        public static readonly BoundKeyFunction MouseMiddle = "MouseMiddle";
        public static readonly BoundKeyFunction RotateObjectClockwise = "RotateObjectClockwise";
        public static readonly BoundKeyFunction RotateObjectCounterclockwise = "RotateObjectCounterclockwise";
        public static readonly BoundKeyFunction FlipObject = "FlipObject";
        public static readonly BoundKeyFunction ToggleRoundEndSummaryWindow = "ToggleRoundEndSummaryWindow";

        public static readonly BoundKeyFunction OpenConsentWindow = "OpenConsentWindow";
        public static readonly BoundKeyFunction OpenEntitySpawnWindow = "OpenEntitySpawnWindow";
        public static readonly BoundKeyFunction OpenSandboxWindow = "OpenSandboxWindow";
        public static readonly BoundKeyFunction OpenTileSpawnWindow = "OpenTileSpawnWindow";
        public static readonly BoundKeyFunction OpenDecalSpawnWindow = "OpenDecalSpawnWindow";
        public static readonly BoundKeyFunction OpenAdminMenu = "OpenAdminMenu";
        public static readonly BoundKeyFunction TakeScreenshot = "TakeScreenshot";
        public static readonly BoundKeyFunction TakeScreenshotNoUI = "TakeScreenshotNoUI";
        public static readonly BoundKeyFunction ToggleFullscreen = "ToggleFullscreen";
        public static readonly BoundKeyFunction Point = "Point";
        public static readonly BoundKeyFunction ZoomOut = "ZoomOut";
        public static readonly BoundKeyFunction ZoomIn = "ZoomIn";
        public static readonly BoundKeyFunction ResetZoom = "ResetZoom";
        public static readonly BoundKeyFunction OfferItem = "OfferItem";
        public static readonly BoundKeyFunction ToggleStanding = "ToggleStanding";
        public static readonly BoundKeyFunction ToggleCrawlingUnder = "ToggleCrawlingUnder";
        public static readonly BoundKeyFunction LookUp = "LookUp";
        public static readonly BoundKeyFunction TargetHead = "TargetHead";
        public static readonly BoundKeyFunction TargetTorso = "TargetTorso";
        public static readonly BoundKeyFunction TargetLeftArm = "TargetLeftArm";
        public static readonly BoundKeyFunction TargetLeftHand = "TargetLeftHand";
        public static readonly BoundKeyFunction TargetRightArm = "TargetRightArm";
        public static readonly BoundKeyFunction TargetRightHand = "TargetRightHand";
        public static readonly BoundKeyFunction TargetLeftLeg = "TargetLeftLeg";
        public static readonly BoundKeyFunction TargetLeftFoot = "TargetLeftFoot";
        public static readonly BoundKeyFunction TargetRightLeg = "TargetRightLeg";
        public static readonly BoundKeyFunction TargetRightFoot = "TargetRightFoot";
        public static readonly BoundKeyFunction ArcadeUp = "ArcadeUp";
        public static readonly BoundKeyFunction ArcadeDown = "ArcadeDown";
        public static readonly BoundKeyFunction ArcadeLeft = "ArcadeLeft";
        public static readonly BoundKeyFunction ArcadeRight = "ArcadeRight";
        public static readonly BoundKeyFunction Arcade1 = "Arcade1";
        public static readonly BoundKeyFunction Arcade2 = "Arcade2";
        public static readonly BoundKeyFunction Arcade3 = "Arcade3";

        public static readonly BoundKeyFunction OpenActionsMenu = "OpenAbilitiesMenu";
        public static readonly BoundKeyFunction ShuttleStrafeLeft = "ShuttleStrafeLeft";
        public static readonly BoundKeyFunction ShuttleStrafeUp = "ShuttleStrafeUp";
        public static readonly BoundKeyFunction ShuttleStrafeRight = "ShuttleStrafeRight";
        public static readonly BoundKeyFunction ShuttleStrafeDown = "ShuttleStrafeDown";
        public static readonly BoundKeyFunction ShuttleRotateLeft = "ShuttleRotateLeft";
        public static readonly BoundKeyFunction ShuttleRotateRight = "ShuttleRotateRight";
        public static readonly BoundKeyFunction ShuttleBrake = "ShuttleBrake";

        public static readonly BoundKeyFunction Hotbar0 = "Hotbar0";
        public static readonly BoundKeyFunction Hotbar1 = "Hotbar1";
        public static readonly BoundKeyFunction Hotbar2 = "Hotbar2";
        public static readonly BoundKeyFunction Hotbar3 = "Hotbar3";
        public static readonly BoundKeyFunction Hotbar4 = "Hotbar4";
        public static readonly BoundKeyFunction Hotbar5 = "Hotbar5";
        public static readonly BoundKeyFunction Hotbar6 = "Hotbar6";
        public static readonly BoundKeyFunction Hotbar7 = "Hotbar7";
        public static readonly BoundKeyFunction Hotbar8 = "Hotbar8";
        public static readonly BoundKeyFunction Hotbar9 = "Hotbar9";

        public static BoundKeyFunction[] GetHotbarBoundKeys() =>
            new[]
            {
                Hotbar1, Hotbar2, Hotbar3, Hotbar4, Hotbar5, Hotbar6, Hotbar7, Hotbar8, Hotbar9, Hotbar0
            };

        public static readonly BoundKeyFunction Vote0 = "Vote0";
        public static readonly BoundKeyFunction Vote1 = "Vote1";
        public static readonly BoundKeyFunction Vote2 = "Vote2";
        public static readonly BoundKeyFunction Vote3 = "Vote3";
        public static readonly BoundKeyFunction Vote4 = "Vote4";
        public static readonly BoundKeyFunction Vote5 = "Vote5";
        public static readonly BoundKeyFunction Vote6 = "Vote6";
        public static readonly BoundKeyFunction Vote7 = "Vote7";
        public static readonly BoundKeyFunction Vote8 = "Vote8";
        public static readonly BoundKeyFunction Vote9 = "Vote9";
        public static readonly BoundKeyFunction EditorCopyObject = "EditorCopyObject";
        public static readonly BoundKeyFunction EditorFlipObject = "EditorFlipObject";
        public static readonly BoundKeyFunction InspectEntity = "InspectEntity";

        public static readonly BoundKeyFunction MappingUnselect = "MappingUnselect";
        public static readonly BoundKeyFunction SaveMap = "SaveMap";
        public static readonly BoundKeyFunction MappingEnablePick = "MappingEnablePick";
        public static readonly BoundKeyFunction MappingEnableDecalPick = "MappingEnableDecalPick";
        public static readonly BoundKeyFunction MappingEnableDelete = "MappingEnableDelete";
        public static readonly BoundKeyFunction MappingPick = "MappingPick";
        public static readonly BoundKeyFunction MappingRemoveDecal = "MappingRemoveDecal";
        public static readonly BoundKeyFunction MappingCancelEraseDecal = "MappingCancelEraseDecal";
        public static readonly BoundKeyFunction MappingOpenContextMenu = "MappingOpenContextMenu";
    }
}
