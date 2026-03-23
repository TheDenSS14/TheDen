// SPDX-FileCopyrightText: 2026 portfiend
//
// SPDX-License-Identifier: MIT

namespace Content.Server._DEN.Speech.Components;

/// <summary>
///     A component that automatically adds text to the start and end of your dialogue.
///     Good for admemes, especially formatting tags.
/// </summary>
/// <remarks>
///     This component is actually a misnomer. Prefix-suffix can be applied *after* accents are
///     processed, so the prefix and suffix doesn't get affected by accent.
/// </remarks>
[RegisterComponent]
public sealed partial class PrefixSuffixAccentComponent : Component
{
    /// <summary>
    ///     Text to add to the start of a dialogue message.
    /// </summary>
    [DataField] public string Prefix = string.Empty;

    /// <summary>
    ///     Text to add to the end of a dialogue message.
    /// </summary>
    [DataField] public string Suffix = string.Empty;

    /// <summary>
    ///     Whether or not this accent should be processed alongside other accents,
    ///     or if it should happen afterwards (preventing your prefix-suffix from being rearranged.)
    /// </summary>
    /// <remarks>
    ///     Keep this "false" if you're using formatting tags!
    /// </remarks>
    [DataField] public bool AffectedByAccents = false;
}
