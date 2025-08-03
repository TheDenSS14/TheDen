// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Net;
using Content.Server.Database;
using Content.Shared.CCVar;
using Content.Shared.FloofStation.Info;
using Robust.Shared.Configuration;
using Robust.Shared.Network;


namespace Content.Server._Floof.Info;


public sealed class NsfwDisclaimerSystem : EntitySystem
{
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IServerDbManager _db = default!;

    public override void Initialize()
    {
        _netManager.RegisterNetMessage<PopupDisclaimerResponseMessage>(OnPopupDisclaimerResponse);

        _netManager.Connected += OnConnected;
    }

    // Why was this handled on the client?
    private void OnPopupDisclaimerResponse(PopupDisclaimerResponseMessage msg)
    {
        if (!msg.Response)
            msg.MsgChannel.Disconnect("User rejected the disclaimer");

        _db.SetAcceptedPrompt(msg.MsgChannel.UserId, msg.Response);
    }

    private async void OnConnected(object? sender, NetChannelArgs e)
    {
        // Ignore localhost unless the specified debug cvar is set
        if (//IPAddress.IsLoopback(e.Channel.RemoteEndPoint.Address)
            //&& _cfg.GetCVar(CCVars.RulesExemptLocal)
            await _db.HasAcceptedPrompt(e.Channel.UserId))
            return;

        var message = new ShowNsfwPopupDisclaimerMessage();
        RaiseNetworkEvent(message, e.Channel);
    }
}
