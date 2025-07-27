// SPDX-FileCopyrightText: 2025 Cami <147159915+Camdot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using System.Text;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class MessageFormatter
{
    private static readonly (string mark, string start, string end, bool onlyNested)[] Rules =
    {
        ("***", "[bolditalic]", "[/bolditalic]", false),
        ("**", "[bold]", "[/bold]", false),
        ("\"", "[color={DialogueColor}]\"", "\"[/color]", false),
        ("*", "[italic]", "[/italic]", true),
        ("*", "[italic][color={EmoteColor}]*", "*[/color][/italic]", false),
    };

    public static string Format(string input, string dialogueColor = "#FFFFFF", string emoteColor = "#FF13FF", bool removeAsterisks = false)
    {
        var result = new StringBuilder();
        var stack = new Stack<string>();
        var inDialogue = false;
        var i = 0;

        while (i < input.Length)
        {
            var (processed, length, dialogueToggled) = ProcessNextToken(input, i, stack, inDialogue, result);

            if (processed)
            {
                i += length;
                if (dialogueToggled)
                    inDialogue = !inDialogue;
                continue;
            }

            result.Append(input[i++]);
        }

        result.Replace("{DialogueColor}", dialogueColor);
        result.Replace("{EmoteColor}", emoteColor);

        if (removeAsterisks)
            result.Replace("*", "");

        return result.ToString();
    }

    private static (bool processed, int length, bool dialogueToggled) ProcessNextToken(
        string input, int position, Stack<string> stack, bool inDialogue, StringBuilder result
    )
    {
        foreach (var rule in Rules.OrderByDescending(r => r.mark.Length))
        {
            if (!CanApplyRule(input, position, rule, inDialogue))
                continue;

            if (rule.mark == "\"")
                return ProcessDialogue(rule, inDialogue, result);

            if (IsClosingTag(stack, rule.mark))
                return ProcessClosingTag(rule, stack, result);

            if (HasMatchingClosingTag(input, position, rule.mark))
                return ProcessOpeningTag(rule, stack, result);

            result.Append(rule.mark);
            return (true, rule.mark.Length, false);
        }

        return (false, 0, false);
    }

    private static bool CanApplyRule(string input, int position, (string mark, string start, string end, bool onlyNested) rule, bool inDialogue)
    {
        if (position + rule.mark.Length > input.Length)
            return false;

        if (input.Substring(position, rule.mark.Length) != rule.mark)
            return false;

        if (rule.onlyNested && !inDialogue)
            return false;

        if (!rule.onlyNested && inDialogue && rule.mark == "*")
            return false;

        return true;
    }

    private static (bool processed, int length, bool dialogueToggled) ProcessDialogue(
        (string mark, string start, string end, bool onlyNested) rule, bool inDialogue, StringBuilder result
    )
    {
        result.Append(inDialogue ? rule.end : rule.start);
        return (true, rule.mark.Length, true);
    }

    private static bool IsClosingTag(Stack<string> stack, string mark) =>
        stack.Count > 0 && stack.Peek() == mark;

    private static (bool processed, int length, bool dialogueToggled) ProcessClosingTag(
        (string mark, string start, string end, bool onlyNested) rule, Stack<string> stack, StringBuilder result
    )
    {
        result.Append(rule.end);
        stack.Pop();
        return (true, rule.mark.Length, false);
    }

    private static bool HasMatchingClosingTag(string input, int position, string mark) =>
        input.IndexOf(mark, position + mark.Length, StringComparison.Ordinal) != -1;

    private static (bool processed, int length, bool dialogueToggled) ProcessOpeningTag(
        (string mark, string start, string end, bool onlyNested) rule, Stack<string> stack, StringBuilder result
    )
    {
        result.Append(rule.start);
        stack.Push(rule.mark);
        return (true, rule.mark.Length, false);
    }

}
