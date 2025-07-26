using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class MessageFormatter
{
    private const char EscapeCharacter = '\\';

    public record FormattingRule(string Mark, string StartTag, string EndTag, bool InsideDialogueOnly, bool IsToggle);

    public class FormatterConfig
    {
        public required List<FormattingRule> Rules { get; set; }

        public required Dictionary<string, string> Replacements { get; set; }

        public bool AllowEscaping { get; set; }

        public required HashSet<char> EscapableTokens { get; set; }

        public bool RemoveAsterisks { get; set; }
    }

    private class FormattingContext
    {
        public StringBuilder Result { get; } = new StringBuilder();
        public Stack<FormattingRule> FormattingStack { get; } = new Stack<FormattingRule>();
        public required IFormattingState CurrentState { get; set; }
    }

    private interface IFormattingState
    {
        FormattingRule[] OrderedRules { get; }
        int ProcessToken(FormattingContext context, string input, int position, FormatterConfig config);
    }

    private class NormalFormattingState : IFormattingState
    {
        public FormattingRule[] OrderedRules { get; }

        public NormalFormattingState(FormatterConfig config)
        {
            OrderedRules = config.Rules.Where(r => !r.InsideDialogueOnly).OrderByDescending(r => r.Mark.Length).ToArray();
        }

        public int ProcessToken(FormattingContext context, string input, int position, FormatterConfig config)
        {
            foreach (var rule in OrderedRules)
            {
                if (!CanApplyRule(input, position, rule))
                    continue;

                if (rule.IsToggle)
                {
                    context.Result.Append(rule.StartTag);
                    context.CurrentState = new DialogueFormattingState(config);
                    return rule.Mark.Length;
                }

                return HandleStackableRule(context, input, position, rule);
            }

            return 0;
        }
    }

    private class DialogueFormattingState : IFormattingState
    {
        public FormattingRule[] OrderedRules { get; }

        public DialogueFormattingState(FormatterConfig config)
        {
            OrderedRules = config.Rules.Where(r => !(!r.InsideDialogueOnly && r.Mark == "*")).OrderByDescending(r => r.Mark.Length).ToArray();
        }

        public int ProcessToken(FormattingContext context, string input, int position, FormatterConfig config)
        {
            foreach (var rule in OrderedRules)
            {
                if (!CanApplyRule(input, position, rule))
                    continue;

                if (rule.IsToggle)
                {
                    context.Result.Append(rule.EndTag);
                    context.CurrentState = new NormalFormattingState(config);
                    return rule.Mark.Length;
                }

                return HandleStackableRule(context, input, position, rule);
            }

            return 0;
        }
    }

    public static string Format(string input, FormatterConfig config)
    {
        var context = new FormattingContext { CurrentState = new NormalFormattingState(config) };
        int i = 0;

        while (i < input.Length)
        {
            if (config.AllowEscaping && TryHandleEscape(input, i, context.Result, config.EscapableTokens, out int consumed))
            {
                i += consumed;
                continue;
            }

            int length = context.CurrentState.ProcessToken(context, input, i, config);
            if (length > 0)
            {
                i += length;
                continue;
            }

            context.Result.Append(input[i++]);
        }

        while (context.FormattingStack.Count > 0)
        {
            var rule = context.FormattingStack.Pop();
            context.Result.Append(rule.EndTag);
        }

        if (context.CurrentState is DialogueFormattingState)
        {
            var toggleRule = config.Rules.First(r => r.IsToggle);
            context.Result.Append(toggleRule.EndTag);
        }

        ApplyReplacements(context.Result, config.Replacements);

        if (config.RemoveAsterisks)
        {
            context.Result.Replace("*", "");
        }

        return context.Result.ToString();
    }

    private static bool TryHandleEscape(string input, int position, StringBuilder result, HashSet<char> escapableTokens, out int consumed)
    {
        consumed = 0;
        if (input[position] != EscapeCharacter || position + 1 >= input.Length)
            return false;

        char next = input[position + 1];
        if (!escapableTokens.Contains(next))
            return false;

        result.Append(next);
        consumed = 2;
        return true;
    }

    private static void ApplyReplacements(StringBuilder result, Dictionary<string, string> replacements)
    {
        foreach (var kvp in replacements)
        {
            result.Replace("{" + kvp.Key + "}", kvp.Value);
        }
    }

    private static bool CanApplyRule(string input, int position, FormattingRule rule)
    {
        if (position + rule.Mark.Length > input.Length)
            return false;

        return input.Substring(position, rule.Mark.Length) == rule.Mark;
    }

    private static int HandleStackableRule(FormattingContext context, string input, int position, FormattingRule rule)
    {
        if (context.FormattingStack.Count > 0 && context.FormattingStack.Peek() == rule)
        {
            context.Result.Append(rule.EndTag);
            context.FormattingStack.Pop();
            return rule.Mark.Length;
        }

        if (HasMatchingClosingTag(input, position, rule.Mark))
        {
            context.Result.Append(rule.StartTag);
            context.FormattingStack.Push(rule);
            return rule.Mark.Length;
        }

        context.Result.Append(rule.Mark);
        return rule.Mark.Length;
    }

    private static bool HasMatchingClosingTag(string input, int position, string mark)
    {
        return FindNextUnescapedMark(input, position + mark.Length, mark) != -1;
    }

    private static int FindNextUnescapedMark(string input, int start, string mark)
    {
        int j = start;
        while (j < input.Length)
        {
            if (input[j] == EscapeCharacter && j + 1 < input.Length)
            {
                j += 2;
                continue;
            }

            if (j + mark.Length <= input.Length && input.Substring(j, mark.Length) == mark)
            {
                return j;
            }

            j++;
        }

        return -1;
    }
}
