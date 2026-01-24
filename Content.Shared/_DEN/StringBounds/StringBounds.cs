// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;

namespace Content.Shared._DEN.StringBounds;

// listen. i needed to make something, okay?
public sealed class StringBounds(string key)
{
    public const string EscapeCharacter = "!";
    public string Key => key;
    public int KeyLength => Key.Length;

    public List<StringBoundsResult> FindWithin(string text)
    {
        var result = new List<StringBoundsResult>();
        var keys = new List<int>();
        var lastSkipped = false;

        for (var i = 0; i < text.Length; i++)
        {
            var currentText = text[i].ToString();
            if (!currentText.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                continue;

            if (lastSkipped)
            {
                lastSkipped = false;
                continue;
            }

            if (i == 0)
            {
                keys.Add(i);
                continue;
            }

            var lastText = text[i - KeyLength].ToString();

            if (lastText.Equals(EscapeCharacter, StringComparison.CurrentCultureIgnoreCase))
            {
                lastSkipped = true;
                continue;
            }

            keys.Add(i);
        }

        var isEndToken = false;
        var lastIndex = 0;
        var keysCopy = new List<int>(keys);

        if (keysCopy.Count == 1)
            return result;

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

        return result;
    }
}

public readonly record struct StringBoundsResult(int StartIndex, int EndIndex, string Result)
{
    public (int StartIndex, int EndIndex) RangeIncludingKey(int keyLength) =>
        (StartIndex - keyLength, EndIndex + keyLength);
}
