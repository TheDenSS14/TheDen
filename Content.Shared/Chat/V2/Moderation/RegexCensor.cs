// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text.RegularExpressions;

namespace Content.Shared.Chat.V2.Moderation;

public sealed class RegexCensor(Regex censorInstruction) : IChatCensor
{
    private readonly Regex _censorInstruction = censorInstruction;

    public bool Censor(string input, out string output, char replaceWith = '*')
    {
        output = _censorInstruction.Replace(input, replaceWith.ToString());

        return !string.Equals(input, output);
    }
}
