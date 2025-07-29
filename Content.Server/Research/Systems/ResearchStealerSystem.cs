// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 SimpleStation14
// SPDX-FileCopyrightText: 2025 Vanessa Louwagie
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Research.Components;
using Content.Shared.Research.Systems;
using Robust.Shared.Random;

namespace Content.Server.Research.Systems;

public sealed class ResearchStealerSystem : SharedResearchStealerSystem
{
    [Dependency] private readonly SharedResearchSystem _research = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ResearchStealerComponent, ResearchStealDoAfterEvent>(OnDoAfter);
    }

    private void OnDoAfter(EntityUid uid, ResearchStealerComponent comp, ResearchStealDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Target == null)
            return;

        var target = args.Target.Value;

        if (!TryComp<TechnologyDatabaseComponent>(target, out var database))
            return;

        var ev = new ResearchStolenEvent(uid, target, new());
        var count = _random.Next(comp.MinToSteal, comp.MaxToSteal - 1); // TheDen - Make the objective a little more tough!
        for (var i = 0; i < count; i++)
        {
            if (database.UnlockedTechnologies.Count == 0)
                break;

            var toRemove = _random.Pick(database.UnlockedTechnologies);
            if (_research.TryRemoveTechnology((target, database), toRemove))
                ev.Techs.Add(toRemove);
        }
        RaiseLocalEvent(uid, ref ev);

        args.Handled = true;
    }
}

/// <summary>
/// Event raised on the user when research is stolen from a RND server.
/// Techs contains every technology id researched.
/// </summary>
[ByRefEvent]
public record struct ResearchStolenEvent(EntityUid Used, EntityUid Target, List<string> Techs);
