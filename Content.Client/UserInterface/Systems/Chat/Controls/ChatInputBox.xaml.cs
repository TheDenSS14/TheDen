// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 2023 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Sk1tch <ben.peter.smith@gmail.com>
// SPDX-FileCopyrightText: 2025 Cami <147159915+Camdot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Stylesheets;
using Content.Client.UserInterface.Systems.Chat.Controls.Denu;
using Content.Shared.Chat;
using Content.Shared.Input;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Chat.Controls;

[Virtual]
public class ChatInputBox : PanelContainer
{
    public readonly ChannelSelectorButton ChannelSelector;
    public readonly HistoryLineEdit Input;
    public readonly ChannelFilterButton FilterButton;
    public readonly DenuButton DenuButton;
    protected readonly BoxContainer Container;
    protected ChatChannel ActiveChannel { get; private set; } = ChatChannel.Local;

    public ChatInputBox()
    {
        Container = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            SeparationOverride = 4
        };
        AddChild(Container);

        ChannelSelector = new ChannelSelectorButton
        {
            Name = "ChannelSelector",
            ToggleMode = true,
            StyleClasses = {"chatSelectorOptionButton"},
            MinWidth = 75
        };
        Container.AddChild(ChannelSelector);
        Input = new HistoryLineEdit
        {
            Name = "Input",
            PlaceHolder = GetChatboxInfoPlaceholder(),
            HorizontalExpand = true,
            StyleClasses = {"chatLineEdit"}
        };
        Container.AddChild(Input);
        FilterButton = new ChannelFilterButton
        {
            Name = "FilterButton",
            StyleClasses = {"chatFilterOptionButton"}
        };
        Container.AddChild(FilterButton);
        DenuButton = new DenuButton
        {
            Name = "DenuButton",
            StyleClasses = {"chatFilterOptionButton"}
        };
        
        Container.AddChild(DenuButton);
        AddStyleClass(StyleNano.StyleClassChatSubPanel);
        ChannelSelector.OnChannelSelect += UpdateActiveChannel;
    }

    private void UpdateActiveChannel(ChatSelectChannel selectedChannel)
    {
        ActiveChannel = (ChatChannel) selectedChannel;
    }

    private static string GetChatboxInfoPlaceholder()
    {
        return (BoundKeyHelper.IsBound(ContentKeyFunctions.FocusChat), BoundKeyHelper.IsBound(ContentKeyFunctions.CycleChatChannelForward)) switch
        {
            (true, true) => Loc.GetString("hud-chatbox-info", ("talk-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.FocusChat)), ("cycle-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.CycleChatChannelForward))),
            (true, false) => Loc.GetString("hud-chatbox-info-talk", ("talk-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.FocusChat))),
            (false, true) => Loc.GetString("hud-chatbox-info-cycle", ("cycle-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.CycleChatChannelForward))),
            (false, false) => Loc.GetString("hud-chatbox-info-unbound")
        };
    }
}
