// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 UBlueberry <161545003+UBlueberry@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class SouthernAccentSystem : EntitySystem
{
    private static readonly Regex RegexIng = new(@"ing\b");
    private static readonly Regex RegexAnd = new(@"\band\b");
    private static readonly Regex RegexDve = new("d've");

    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SouthernAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, SouthernAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = _replacement.ApplyReplacements(message, "southern");

        //They shoulda started runnin' an' hidin' from me!
        message = RegexIng.Replace(message, "in'");
        message = RegexAnd.Replace(message, "an'");
        message = RegexDve.Replace(message, "da");
        args.Message = message;
    }
};
