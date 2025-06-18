// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Examine;

namespace Content.Shared.Abilities.Psionics;

public sealed class MindbrokenSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MindbrokenComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, MindbrokenComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup($"[color=mediumpurple]{Loc.GetString(component.MindbrokenExaminationText, ("entity", uid))}[/color]");
    }
}