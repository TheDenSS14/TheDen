// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 BombasterDS
// SPDX-FileCopyrightText: 2025 BombasterDS2
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Language;
using Content.Shared.Speech;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.TapeRecorder;

/// <summary>
/// Every chat event recorded on a tape is saved in this format
/// </summary>
[DataDefinition]
public sealed partial class TapeCassetteRecordedMessage : IComparable<TapeCassetteRecordedMessage>
{
    /// <summary>
    /// Number of seconds since the start of the tape that this event was recorded at
    /// </summary>
    [DataField(required: true)]
    public float Timestamp;

    /// <summary>
    /// The name of the entity that spoke
    /// </summary>
    [DataField]
    public string? Name;

    /// <summary>
    /// The verb used for this message.
    /// </summary>
    [DataField]
    public ProtoId<SpeechVerbPrototype>? Verb;

    /// <summary>
    /// What was spoken
    /// </summary>
    [DataField]
    public string Message = string.Empty;

    /// <summary>
    /// The language of what was spoken
    /// </summary>
    [DataField]
    public ProtoId<LanguagePrototype>? Language;

    public TapeCassetteRecordedMessage(
        float timestamp,
        string name,
        ProtoId<SpeechVerbPrototype> verb,
        string message,
        ProtoId<LanguagePrototype>? language)
    {
        Timestamp = timestamp;
        Name = name;
        Verb = verb;
        Message = message;
        Language = language;
    }

    public int CompareTo(TapeCassetteRecordedMessage? other)
    {
        if (other == null)
            return 0;

        return (int) (Timestamp - other.Timestamp);
    }
}
