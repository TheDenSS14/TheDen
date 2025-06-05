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
            string? color = null;

            var separateNameAndMessage = false;
            var args = arrayArgs.ToList();

            if (args.Count < 3)
            {
                shell.WriteError("You must provide at least 3 arguments.");
                return;
            }

            if (args[0] != "none" && args[0].Length > 6 && args[0].Length < 8)
            {
                if (args[0].StartsWith("#") && args[0].Length == 7)
                    color = args[0];

                if (args[0].Length == 6)
                    color = $"#{args[0]}";
            }

            if (args[1].ToLower() == "true")
            {
                separateNameAndMessage = true;
            }

            if (args[1].ToLower() == "false" && color == null) // scuffed
                args.RemoveAt(1);

            if (separateNameAndMessage || color != null)
            {
                args.RemoveAt(0);
                args.RemoveAt(0);
            }

            SubtleUtilities.RunSubtle(shell, args.ToArray(), color ?? "#d3d3ff", separateNameAndMessage);
        }
    }
}
