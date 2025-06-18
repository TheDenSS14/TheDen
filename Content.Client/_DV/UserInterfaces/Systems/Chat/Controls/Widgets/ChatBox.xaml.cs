// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Client.UserInterface.Systems.Chat.Widgets;

public partial class ChatBox
{
    private void OnNewHighlights(string highlights)
    {
        _controller.UpdateHighlights(highlights);
    }
}
