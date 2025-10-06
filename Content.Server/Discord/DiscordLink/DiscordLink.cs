// SPDX-FileCopyrightText: 2025 Simon
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Text;
using System.Threading.Tasks;
using Content.Server._DEN.Discord;
using Content.Shared.CCVar;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using Robust.Shared.Configuration;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Content.Server.Discord.DiscordLink;

/// <summary>
/// Represents the arguments for the <see cref="DiscordLink.OnCommandReceived"/> event.
/// </summary>
public sealed class CommandReceivedEventArgs
{
    /// <summary>
    /// The command that was received. This is the first word in the message, after the bot prefix.
    /// </summary>
    public string Command { get; init; } = string.Empty;

    /// <summary>
    /// The arguments to the command.
    /// </summary>
    public required DiscordArguments Arguments { get; init; }

    /// <summary>
    /// Information about the message that the command was received from. This includes the message content, author, etc.
    /// Use this to reply to the message, delete it, etc.
    /// </summary>
    public Message Message { get; init; } = default!;
}

/// <summary>
/// Handles the connection to Discord and provides methods to interact with it.
/// </summary>
public sealed class DiscordLink : IPostInjectInit
{
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly IConfigurationManager _configuration = default!;

    /// <summary>
    ///    The Discord client. This is null if the bot is not connected.
    /// </summary>
    /// <remarks>
    ///     This should not be used directly outside of DiscordLink. So please do not make it public. Use the methods in this class instead.
    /// </remarks>
    private GatewayClient? _client;
    private ISawmill _sawmill = default!;
    private ISawmill _sawmillLog = default!;

    private ulong _guildId;
    private string _botToken = string.Empty;

    public string BotPrefix = default!;
    /// <summary>
    /// If the bot is currently connected to Discord.
    /// </summary>
    public bool IsConnected => _client != null;

    #region Events

    /// <summary>
    ///     Event that is raised when a command is received from Discord.
    /// </summary>
    public event Action<CommandReceivedEventArgs>? OnCommandReceived;
    /// <summary>
    ///     Event that is raised when a message is received from Discord. This is raised for every message, including commands.
    /// </summary>
    public event Action<Message>? OnMessageReceived;

    public void RegisterCommandCallback(Action<CommandReceivedEventArgs> callback, string command)
    {
        OnCommandReceived += args =>
        {
            if (args.Command == command)
                callback(args);
        };
    }

    #endregion

    public void Initialize()
    {
        _configuration.OnValueChanged(CCVars.DiscordGuildId, OnGuildIdChanged, true);
        _configuration.OnValueChanged(CCVars.DiscordPrefix, OnPrefixChanged, true);

        if (_configuration.GetCVar(CCVars.DiscordToken) is not { } token || token == string.Empty)
        {
            _sawmill.Info("No Discord token specified, not connecting.");
            return;
        }

        // If the Guild ID is empty OR the prefix is empty, we don't want to connect to Discord.
        if (_guildId == 0 || BotPrefix == string.Empty)
        {
            // This is a warning, not info, because it's a configuration error.
            // It is valid to not have a Discord token set which is why the above check is an info.
            // But if you have a token set, you should also have a guild ID and prefix set.
            _sawmill.Warning("No Discord guild ID or prefix specified, not connecting.");
            return;
        }

        _client = new GatewayClient(new BotToken(token), new GatewayClientConfiguration()
        {
            Intents = GatewayIntents.All,
            Logger = new DiscordSawmillLogger(_sawmillLog)
        });
        _client.MessageCreate += OnCommandReceivedInternal;
        _client.MessageCreate += OnMessageReceivedInternal;

        _botToken = token;
        // Since you cannot change the token while the server is running / the DiscordLink is initialized,
        // we can just set the token without updating it every time the cvar changes.

        _client.Ready += _ =>
        {
            _sawmill.Info("Discord client ready.");
            return default;
        };

        Task.Run(async () =>
        {
            try
            {
                await _client.StartAsync();
                _sawmill.Info("Connected to Discord.");
            }
            catch (Exception e)
            {
                _sawmill.Error("Failed to connect to Discord!\n" + e);
            }
        });
    }

    public async Task Shutdown()
    {
        if (_client != null)
        {
            _sawmill.Info("Disconnecting from Discord.");

            // Unsubscribe from the events.
            _client.MessageCreate -= OnCommandReceivedInternal;
            _client.MessageCreate -= OnMessageReceivedInternal;

            await _client.CloseAsync();
            _client.Dispose();
            _client = null;
        }

        _configuration.UnsubValueChanged(CCVars.DiscordGuildId, OnGuildIdChanged);
        _configuration.UnsubValueChanged(CCVars.DiscordPrefix, OnPrefixChanged);
    }

    public async void ReloadBot()
    {
        await Shutdown();
        Initialize();
    }

    void IPostInjectInit.PostInject()
    {
        _sawmill = _logManager.GetSawmill("discord.link");
        _sawmillLog = _logManager.GetSawmill("discord.link.log");
    }

    public GatewayClient? Client => _client;
    public ulong GuildId => _guildId;

    private void OnGuildIdChanged(string guildId)
    {
        _guildId = ulong.TryParse(guildId, out var id) ? id : 0;
    }

    private void OnPrefixChanged(string prefix)
    {
        BotPrefix = prefix;
    }

    private ValueTask OnCommandReceivedInternal(Message message)
    {
        var content = message.Content;
        // If the message doesn't start with the bot prefix, ignore it.
        if (!content.StartsWith(BotPrefix))
            return ValueTask.CompletedTask;

        // Split the message into the command and the arguments.
        var trimmedInput = content[BotPrefix.Length..].Trim();
        var firstSpaceIndex = trimmedInput.IndexOf(' ');

        string command, rawArguments;

        if (firstSpaceIndex == -1)
        {
            command = trimmedInput;
            rawArguments = string.Empty;
        }
        else
        {
            command = trimmedInput[..firstSpaceIndex];
            rawArguments = trimmedInput[(firstSpaceIndex + 1)..].Trim();
        }

        var arguments = GetArgumentsFromString(rawArguments);
        var discordArguments = new DiscordArguments(rawArguments, arguments);

        // Raise the event!
        OnCommandReceived?.Invoke(new CommandReceivedEventArgs
        {
            Command = command,
            Arguments = discordArguments,
            Message = message
        });
        return ValueTask.CompletedTask;
    }

    private ValueTask OnMessageReceivedInternal(Message message)
    {
        OnMessageReceived?.Invoke(message);
        return ValueTask.CompletedTask;
    }

    private List<string> GetArgumentsFromString(string input)
    {
        var result = new List<string>();
        var initialSplit = input.Split(' ');

        if (initialSplit.Length < 1)
            return result;

        var startedQuote = int.MaxValue;
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < initialSplit.Length; i++)
        {
            var element = initialSplit[i];
            var hasQuote = element.StartsWith('"') || element.StartsWith('\'');
            var endsInQuote = element.EndsWith('"') || element.EndsWith('\'');

            // The active quote check ended. Reset.
            if (startedQuote != int.MaxValue && endsInQuote)
            {
                var count = (i - 1) - startedQuote;

                element = element.Replace("\"", string.Empty)
                    .Replace("'", string.Empty);

                result.RemoveRange(startedQuote, count); // Remove safety measure elements
                stringBuilder.Append($" {element}");
                result.Add(stringBuilder.ToString());
                startedQuote = int.MaxValue;
                continue;
            }

            // If there is an active quote check, everything after the quote should be a part of it.
            if (startedQuote != int.MaxValue)
            {
                stringBuilder.Append($" {element}");
                result.Add(element); // If it never ends, we'll just use this.
                continue;
            }

            // There's no active quote check; make one.
            if (startedQuote == int.MaxValue && hasQuote)
            {
                element = element.Replace("\"", string.Empty)
                    .Replace("'", string.Empty);

                stringBuilder.Clear();
                stringBuilder.Append(element);
                startedQuote = i;
                continue;
            }

            result.Add(element);
        }

        return result;
    }

    #region Proxy methods

    /// <summary>
    /// Sends a message to a Discord channel with the specified ID. Without any mentions.
    /// </summary>
    public async Task SendMessageAsync(ulong channelId, string message)
    {
        if (_client == null)
        {
            return;
        }

        var channel = await _client.Rest.GetChannelAsync(channelId) as TextChannel;
        if (channel == null)
        {
            _sawmill.Error("Tried to send a message to Discord but the channel {Channel} was not found.", channel);
            return;
        }

        await channel.SendMessageAsync(new MessageProperties()
        {
            AllowedMentions = AllowedMentionsProperties.None,
            Content = message,
        });
    }

    #endregion
}
