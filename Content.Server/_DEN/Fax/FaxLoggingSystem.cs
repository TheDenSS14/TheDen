using System.Threading.Tasks;
using Content.Server.Discord.DiscordLink;
using Content.Server.GameTicking;
using Content.Server.Paper;
using Content.Shared._DEN.Fax;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Utility;


namespace Content.Server._DEN.Fax;


/// <summary>
/// This handles sending discord logs of CentComm faxes.
/// </summary>
public sealed class FaxLoggingSystem : EntitySystem
{
    [Dependency] private readonly DiscordLink _discordLink = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    private ISawmill _sawmill = default!;
    private string? _discordFaxChannelId;
    private const int DiscordMaxMessageLength = 2000;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<FaxSentEvent>(OnFaxSent);

        var channelId = _cfg.GetCVar(CCVars.DiscordFaxChannelId);
        _discordFaxChannelId = channelId;

        _sawmill = _log.GetSawmill("faxlogging");
        _cfg.OnValueChanged(CCVars.DiscordFaxChannelId, OnCVarChanged, true);
    }

    private void OnCVarChanged(string newChannelId)
    {
        _discordFaxChannelId = newChannelId;
    }

    private void OnFaxSent(FaxSentEvent msg)
    {
        if (string.IsNullOrWhiteSpace(msg.Content))
        {
            _sawmill.Info("Failed to send fax message to discord.");
            return;
        }

        var content = FormattedMessage.RemoveMarkupPermissive(msg.Content);
        SendToWebhook(msg);
    }

    private async void SendToWebhook(FaxSentEvent msg)
    {
        var content = msg.Content;
        var receivingFrom = msg.DestinationAddress;

        var slices = new List<string>();
        var sliceCount = Math.Ceiling((double) content.Length / DiscordMaxMessageLength);
        var contentLeft = content;
        var lastSliceIndex = 0;

        for (var i = 0; i < sliceCount; i++)
        {
            var remaining = contentLeft.Length - 1;
            var slicesToTake = Math.Min(DiscordMaxMessageLength - 1, remaining);
            var slice = content.Substring(lastSliceIndex + 1, slicesToTake);
            slices.Add(slice);

            if (contentLeft.Length - slicesToTake + 1 > 0)
                contentLeft = contentLeft.Substring(slicesToTake);
            else
                contentLeft = string.Empty;

            lastSliceIndex = slicesToTake;
        }

        var title = $"# Incoming Fax from {receivingFrom}";

        if (msg.StampedBy.Count > 0)
        {
            var stamps = string.Join(", ", msg.StampedBy);
            title += $"\n-# Stamps ({msg.StampedBy.Count}): {stamps}";
            title += $"\n-# Round ID: #{_gameTicker.RoundId}";
        }

        await SendToDiscordWebhook(title);

        foreach (var slice in slices)
            await SendToDiscordWebhook(slice);
    }

    private async Task SendToDiscordWebhook(string content)
    {
        if (string.IsNullOrWhiteSpace(_discordFaxChannelId)
            || !ulong.TryParse(_discordFaxChannelId, out var channelId))
        {
            _sawmill.Info("No channel ID configured.");
            return;
        }

        _sawmill.Info("Sending fax to discord. Token: " + _discordFaxChannelId);

        try
        {
            await _discordLink.SendMessageAsync(channelId, content);
        }
        catch (Exception e)
        {
            _sawmill.Error($"Failed to send fax to discord:\n{e}");
        }
    }
}
