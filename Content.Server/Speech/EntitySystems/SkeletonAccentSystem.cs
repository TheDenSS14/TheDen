// SPDX-FileCopyrightText: 2022 Willhelm53 <97707302+Willhelm53@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed partial class SkeletonAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [GeneratedRegex(@"(?<!\w)[^aeiou]one", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex BoneRegex();

    private static readonly Dictionary<string, string> DirectReplacements = new()
    {
        { "fuck you", "I've got a BONE to pick with you" },
        { "fucked", "boned"},
        { "fuck", "RATTLE RATTLE" },
        { "fck", "RATTLE RATTLE" },
        { "shit", "RATTLE RATTLE" }, // Capitalize RATTLE RATTLE regardless of original message case.
        { "definitely", "make no bones about it" },
        { "absolutely", "make no bones about it" },
        { "afraid", "rattled"},
        { "scared", "rattled"},
        { "spooked", "rattled"},
        { "shocked", "rattled"},
        { "killed", "skeletonized"},
        { "humorous", "humerus"},
        { "to be a", "tibia"},
        { "under", "ulna"},
        { "narrow", "marrow"},
    };

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkeletonAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message, SkeletonAccentComponent component)
    {
        // Order:
        // Do character manipulations first
        // Then direct word/phrase replacements
        // Then prefix/suffix

        var msg = message;

        // Character manipulations:
        // At the start of words, any non-vowel + "one" becomes "bone", e.g. tone -> bone ; lonely -> bonely; clone -> clone (remains unchanged).
        msg = BoneRegex().Replace(msg, "bone");

        // Direct word/phrase replacements:
        foreach (var (first, replace) in DirectReplacements)
        {
            msg = Regex.Replace(msg, $@"(?<!\w){first}(?!\w)", replace, RegexOptions.IgnoreCase);
        }

        // Suffix:
        if (_random.Prob(component.ackChance))
        {
            msg += (" " + Loc.GetString("skeleton-suffix")); // e.g. "We only want to socialize. ACK ACK!"
        }
        return msg;
    }

    private void OnAccentGet(EntityUid uid, SkeletonAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}
