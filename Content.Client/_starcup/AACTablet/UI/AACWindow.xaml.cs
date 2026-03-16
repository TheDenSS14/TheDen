// SPDX-FileCopyrightText: 2025 little-meow-meow
// SPDX-FileCopyrightText: 2026 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._starcup.AACTablet;
using Content.Shared.Chat;
using Content.Shared.Radio;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._DV.AACTablet.UI;

public sealed partial class AACWindow
{
    private string Prefix => (string?) RadioChannels.SelectedMetadata ?? SharedChatSystem.LocalPrefix.ToString();

    internal void Update(AACTabletBuiState msg)
    {
        RadioChannels.Clear();

        var id = 0;
        AddChannel(Loc.GetString("hud-chatbox-channel-Local"), // DEN: use helper function
            SharedChatSystem.LocalPrefix.ToString(),
            ref id);
        AddChannel(Loc.GetString("hud-chatbox-channel-Whisper"), // DEN: add whisper
            SharedChatSystem.WhisperPrefix.ToString(),
            ref id);

        foreach (var channel in msg.RadioChannels)
        {
            var channelProto = _prototype.Index<RadioChannelPrototype>(channel);
            // DEN start: use helper function
            var prefix = string.Concat(SharedChatSystem.RadioChannelPrefix, channelProto.KeyCode);

            AddChannel(channelProto.LocalizedName, prefix, ref id);
            // DEN end
        }
    }

    // DEN start: helper function
    private void AddChannel(string channelName, string prefix, ref int id)
    {
        RadioChannels.AddItem(channelName, id);
        RadioChannels.SetItemMetadata(RadioChannels.GetIdx(id), prefix);
        id++;
    }
    // DEN end

    private void OnChannelSelected(OptionButton.ItemSelectedEventArgs args)
    {
        RadioChannels.SelectId(args.Id);
    }
}
