// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 fox
// SPDX-FileCopyrightText: 2025 Rocket
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Text;
using Content.Shared._DEN.StringBounds;
using Content.Shared.GameTicking;
using Robust.Shared.Prototypes;

namespace Content.Shared.Language.Systems;

public abstract class SharedLanguageSystem : EntitySystem
{
    /// <summary>
    ///     The language used as a fallback in cases where an entity suddenly becomes a language speaker (e.g. the usage of make-sentient)
    /// </summary>
    public static readonly ProtoId<LanguagePrototype> FallbackLanguagePrototype = "TauCetiBasic";

    /// <summary>
    ///     The language whose speakers are assumed to understand and speak every language. Should never be added directly.
    /// </summary>
    public static readonly ProtoId<LanguagePrototype> UniversalPrototype = "Universal";

    /// <summary>
    ///     Language used for Xenoglossy, should have same effects as Universal but with different Language prototype
    /// </summary>
    public static readonly ProtoId<LanguagePrototype> PsychomanticPrototype = "Psychomantic";

    /// <summary>
    /// A cached instance of <see cref="PsychomanticPrototype"/>
    /// </summary>
    public static LanguagePrototype Psychomantic { get; private set; } = default!;

    /// <summary>
    ///     A cached instance of <see cref="UniversalPrototype"/>
    /// </summary>
    public static LanguagePrototype Universal { get; private set; } = default!;

    [Dependency] protected readonly IPrototypeManager _prototype = default!;
    [Dependency] protected readonly SharedGameTicker _ticker = default!;

    private readonly StringBounds _dialogueStringBounds = new("\"");

    public override void Initialize()
    {
        Universal = _prototype.Index(UniversalPrototype);
         // Initialize the Psychomantic prototype
        Psychomantic = _prototype.Index(PsychomanticPrototype);
    }

    public LanguagePrototype? GetLanguagePrototype(ProtoId<LanguagePrototype> id)
    {
        _prototype.TryIndex(id, out var proto);
        return proto;
    }

    /// <summary>
    ///     Obfuscate a message using the given language.
    /// </summary>
    public string ObfuscateSpeech(string message, LanguagePrototype language)
    {
        var builder = new StringBuilder();
        language.Obfuscation.Obfuscate(builder, message, this);

        return builder.ToString();
    }

    /// <summary>
    ///     Gets all text within dialogue.
    /// </summary>
    /// <param name="text">The text to find the keys that are within dialogue.</param>
    /// <returns>A list of <see cref="StringBoundsResult"/> containing all keys and the relevant ranges.</returns>
    public List<StringBoundsResult> GetKeysWithinDialogue(string text)
    {
        var keysWithinDialogue = _dialogueStringBounds.FindWithin(text);
        return keysWithinDialogue;
    }

    /// <summary>
    ///     Generates a stable pseudo-random number in the range (min, max) (inclusively) for the given seed.
    ///     One seed always corresponds to one number, however the resulting number also depends on the current round number.
    ///     This method is meant to be used in <see cref="ObfuscationMethod"/> to provide stable obfuscation.
    /// </summary>
    internal int PseudoRandomNumber(int seed, int min, int max)
    {
        // Using RobustRandom or System.Random here is a bad idea because this method can get called hundreds of times per message.
        // Each call would require us to allocate a new instance of random, which would lead to lots of unnecessary calculations.
        // Instead, we use a simple but effective algorithm derived from the C language.
        // It does not produce a truly random number, but for the purpose of obfuscating messages in an RP-based game it's more than alright.
        seed = seed ^ (_ticker.RoundId * 127);
        var random = seed * 1103515245 + 12345;
        return min + Math.Abs(random) % (max - min + 1);
    }
}
