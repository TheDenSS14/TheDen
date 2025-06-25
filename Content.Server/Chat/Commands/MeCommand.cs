// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 Clyybber <darkmine956@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 HerCoyote23 <131214189+HerCoyote23@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Chat.Commands;
using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Content.Shared.Chat;
using Robust.Shared.Console;
using Robust.Shared.Enums;

namespace Content.Server.Chat.Commands
{
    internal sealed class MeUtilities
    {
        public static void RunMe(IConsoleShell shell, string[] args, bool separate = false)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError("This command cannot be run from the server.");
                return;
            }

            if (player.Status != SessionStatus.InGame)
                return;

            if (player.AttachedEntity is not {} playerEntity)
            {
                shell.WriteError("You don't have an entity!");
                return;
            }

            if (args.Length < 1)
                return;

            var message = string.Join(" ", args).Trim();

            if (string.IsNullOrEmpty(message))
                return;

            var sysManager = IoCManager.Resolve<IEntitySystemManager>();
            var chatSystem = sysManager.GetEntitySystem<ChatSystem>();

            chatSystem.TrySendInGameICMessage(
                playerEntity,
                message,
                InGameICChatType.Emote,
                ChatTransmitRange.Normal,
                false,
                shell,
                player,
                separateNameAndMessage: separate);
        }
    }

    [AnyCommand]
    internal sealed class MeCommand : IConsoleCommand
    {
        public string Command => "me";
        public string Description => "Perform an action.";
        public string Help => "me <text>";

        public void Execute(IConsoleShell shell, string argStr, string[] args) => MeUtilities.RunMe(shell, args);
    }

    [AnyCommand]
    internal sealed class MeDetailedCommand : IConsoleCommand
    {
        public string Command => "medetailed";
        public string Description => "Perform an action. If separate is true, your emote will display as (Name) Message instead of Name Message.";
        public string Help => "medetailed <separate (true/false)> <text>";

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                return CompletionResult.FromOptions(["true", "false"]);
            }

            if (args.Length >= 2)
            {
                return CompletionResult.FromHint("[ message ]");
            }

            return CompletionResult.Empty;
        }

        public void Execute(IConsoleShell shell, string argStr, string[] arrayArgs)
        {
            var separateNameAndMessage = false;
            var args = arrayArgs.ToList();

            if (args.Count < 2)
            {
                shell.WriteError("You must provide at least 2 arguments.");
                return;
            }

            switch (args[0].ToLower())
            {
                case "true":
                    separateNameAndMessage = true;
                    args.RemoveAt(0);
                    break;
                case "false":
                    args.RemoveAt(0);
                    break;
            }

            MeUtilities.RunMe(shell, args.ToArray(), separateNameAndMessage);
        }
    }
}
