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
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 Cami <147159915+Camdot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Content.Shared.Chat;
using Robust.Shared.Console;
using Robust.Shared.Enums;

namespace Content.Server.Chat.Commands
{
    internal sealed class SubtleUtilities
    {
        private const string DefaultSubtleColor = "#d3d3ff";

        public static void RunSubtle(IConsoleShell shell, string[] args, string color = DefaultSubtleColor, bool separate = false)
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
                InGameICChatType.Subtle,
                ChatTransmitRange.NoGhosts,
                false,
                shell,
                player,
                color: color,
                separateNameAndMessage: separate);
        }
    }

    [AnyCommand]
    internal sealed class SubtleCommand : IConsoleCommand
    {
        public string Command => "subtle";
        public string Description => "Perform a subtle action.";
        public string Help => "subtle <text>";

        public void Execute(IConsoleShell shell, string argStr, string[] args) => SubtleUtilities.RunSubtle(shell, args);
    }

    [AnyCommand]
    internal sealed class SubtleDetailedCommand : IConsoleCommand
    {
        public string Command => "subtledetailed";
        public string Description => "Perform a subtle action with your own color and option for whether there's a space between name and message.";
        public string Help => "subtledetailed <color or 'none' for default> <separate (true/false)> <text>";

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                return CompletionResult.FromHint("#ffffff");
            }

            if (args.Length == 2)
            {
                return CompletionResult.FromOptions(["true", "false"]);
            }

            if (args.Length >= 3)
            {
                return CompletionResult.FromHint("[ message ]");
            }

            return CompletionResult.Empty;
        }

        public void Execute(IConsoleShell shell, string argStr, string[] arrayArgs)
        {
            var args = argStr.Split(" ");

            if (args.Length < 4)
            {
                shell.WriteError("You must provide at least 3 arguments.");
                return;
            }

            string colorStr = args[1];
            bool separateNameAndMessage = args[2].ToLower() == "true";
            
            if (!colorStr.StartsWith("#"))
                colorStr = "#" + colorStr;

            if (!Color.TryParse(colorStr, out _))
                colorStr = "#d3d3ff";
            
            var messageArr = args.Skip(3).ToArray();
            
            SubtleUtilities.RunSubtle(shell, messageArr, colorStr, separateNameAndMessage);
        }
    }
}
