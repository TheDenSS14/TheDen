// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MonotonousSystem : EntitySystem
{
    // @formatter:off
    private static readonly Regex RegexAnyPunctuationNotPeriod = new(@"[!?]+");
    // @formatter:on

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MonotonousComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, MonotonousComponent component, AccentGetEvent args)
    {
        args.Message = RegexAnyPunctuationNotPeriod.Replace(args.Message, ".");

        // If the message doesn't end with a letter or punctuation, we add a period for sharpness
        if (!char.IsLetterOrDigit(args.Message[^1]) && !char.IsPunctuation(args.Message[^1]))
        {
            args.Message += ".";
        }
    }
}
