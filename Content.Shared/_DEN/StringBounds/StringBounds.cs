using System.Linq;

namespace Content.Shared._DEN.StringBounds;

// listen. i needed to make something, okay?
public sealed class StringBounds(string key)
{
    public const string EscapeCharacter = "\\";
    public string Key => key;
    public int KeyLength => Key.Length;

    public List<StringBoundsResult> FindWithin(string text)
    {
        var result = new List<StringBoundsResult>();
        var keys = new List<int>();

        for (var i = 0; i < text.Length; i++)
        {
            var currentText = text[i].ToString();
            if (!currentText.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                continue;

            if (i == 0)
            {
                keys.Add(i);
                continue;
            }

            var lastText = text[i - KeyLength].ToString();

            if (lastText.Equals(EscapeCharacter, StringComparison.CurrentCultureIgnoreCase))
                continue;

            keys.Add(i);
        }

        var isEndToken = false;
        var lastIndex = 0;
        var keysCopy = new List<int>(keys);

        foreach (var foundKeyIndex in keysCopy)
        {
            if (isEndToken)
            {
                var normalizedStart = lastIndex + KeyLength;
                var normalizedEnd = foundKeyIndex;
                var normalizedLength = foundKeyIndex - lastIndex - KeyLength;
                var resultingText = text.Substring(normalizedStart, normalizedLength);
                var stringBoundsResult = new StringBoundsResult(normalizedStart, normalizedEnd, resultingText);

                result.Add(stringBoundsResult);
                keys.Remove(lastIndex);
                keys.Remove(foundKeyIndex);

                isEndToken = false;
                continue;
            }

            lastIndex = foundKeyIndex;
            isEndToken = true;
        }

        // if we have an unending key
        if (keys.Count == 1)
        {
            var start = keys.First();
            var resultingText = text.Substring(start);
            var nonEndingResult = new StringBoundsResult(keys[0], resultingText.Length - 1, resultingText);

            result.Add(nonEndingResult);
        }

        return result;
    }
}

public readonly record struct StringBoundsResult(int StartIndex, int EndIndex, string Result)
{
    public (int StartIndex, int EndIndex) RangeIncludingKey(int keyLength) =>
        (StartIndex - keyLength, EndIndex + keyLength);
}
