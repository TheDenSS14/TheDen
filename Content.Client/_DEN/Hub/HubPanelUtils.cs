// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Client.Message;
using Content.Client.UserInterface.Controls;
using Content.Shared._DEN.Hub;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using HAlignment = Robust.Client.UserInterface.Control.HAlignment;

namespace Content.Client._DEN.Hub;

public sealed class HubPanelUtils
{
    public static RichTextLabel SetupTitleLabel(HubServer server)
    {
        var titleLabel = new RichTextLabel
        {
            Margin = new(3, 3),
            HorizontalExpand = true,
            VerticalExpand = true,
            HorizontalAlignment = Control.HAlignment.Center
        };

        var displayed = server.DisplayName ?? server.ServerId;
        var titleLabelText = Loc.GetString(
            "hub-panel-server-name",
            ("displayName", displayed));

        titleLabel.SetMarkupPermissive(titleLabelText);
        return titleLabel;
    }

    public static RichTextLabel SetupStatusLabel(HubServer server)
    {
        var statusText = server.IsOnline ? "hub-panel-status-text-online" : "hub-panel-status-text-offline";
        var statusLabel = new RichTextLabel
        {
            HorizontalAlignment = HAlignment.Center,
            HorizontalExpand = true,
            VerticalExpand = true,
            Text = Loc.GetString(statusText)
        };

        return statusLabel;
    }

    public static RichTextLabel SetupPlayersLabel(HubServer server)
    {
        var playersLabel = new RichTextLabel
        {
            HorizontalAlignment = HAlignment.Center,
            HorizontalExpand = true,
            VerticalExpand = true,
        };

        var players = server.Players ?? 0;
        var maxPlayers = server.MaxPlayers ?? 0;

        var playersText = Loc.GetString(
            "hub-panel-status-players",
            ("players", players),
            ("maxPlayers", maxPlayers));

        playersLabel.SetMarkupPermissive(playersText);
        return playersLabel;
    }

    public static Button SetupConnectButton(HubServer server, string serverId, string connectFriendly)
    {
        var sameServer = serverId == server.ServerId;

        var tooltipId = sameServer
            ? "hub-panel-connect-button-same-server-tooltip"
            : "hub-panel-connect-button-tooltip";

        if (!server.CanConnect)
            tooltipId = "hub-panel-connect-button-not-allowed-tooltip";

        var tooltipText = Loc.GetString(tooltipId, ("address", connectFriendly));

        var buttonTextId = sameServer
            ? "hub-panel-connect-button-same-server-text"
            : "hub-panel-connect-button-text";

        var connectButton = new ConfirmButton
        {
            Text = Loc.GetString(buttonTextId),
            ToolTip = tooltipText,
            MaxHeight = 35,
            TextAlign = Label.AlignMode.Center,
            Disabled = server.ServerId == serverId || !server.CanConnect || !server.IsOnline
        };

        return connectButton;
    }

    public static StripeBack CreateColumnCategory(LocId titleId)
    {
        var stripeBack = new StripeBack();
        var text = Loc.GetString(titleId);

        var titleLabel = new RichTextLabel
        {
            Text = text,
            HorizontalAlignment = HAlignment.Center
        };

        stripeBack.AddChild(titleLabel);
        return stripeBack;
    }
}
