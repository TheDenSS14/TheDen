using System.Diagnostics.CodeAnalysis;


namespace Content.Server._DEN.Discord;


public sealed class DiscordArguments(string rawArguments, List<string> arguments)
{
    public string RawArguments { get; set; } = rawArguments;
    public List<string> Arguments { get; set; } = arguments;

    public bool HasArgument(int index)
    {
        if (index >= Arguments.Count)
            return false;

        var arg = Arguments[index];
        return !string.IsNullOrWhiteSpace(arg);
    }

    public bool IsArgument(int index, string value)
    {
        if (!HasArgument(index))
            return false;

        return Arguments[index] == value;
    }

    public bool TryGetIntArgument(int index, out int value, int defaultValue = 0)
    {
        if (!HasArgument(index))
        {
            value = defaultValue;
            return false;
        }

        var element = Arguments[index];
        var result = int.TryParse(element, out value);

        return result;
    }

    public bool TryGetBoolArgument(int index, out bool value, bool defaultValue = false)
    {
        if (!HasArgument(index))
        {
            value = defaultValue;
            return false;
        }

        var element = Arguments[index];
        var result = bool.TryParse(element, out value);

        return result;
    }

    public string GetArgument(int index) => Arguments[index];
}
