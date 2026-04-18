// SPDX-FileCopyrightText: 2024 LankLTE
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 somekindofbox
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Body.Systems;
using Content.Shared.Chat.TypingIndicator;

namespace Content.Server._CD.Traits;

public sealed class SynthSystem : EntitySystem
{
    [Dependency] private readonly BloodstreamSystem _bloodstream = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SynthComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, SynthComponent component, ComponentStartup args)
    {
        if (TryComp<TypingIndicatorComponent>(uid, out var indicator))
        {
            indicator.Prototype = "robot";
            Dirty(uid, indicator);
        }

        // Give them synth blood. Ion storm notif is handled in that system
        // _bloodstream.ChangeBloodReagent(uid, "SynthBlood"); DEN Change - Blood refactoring, killing duplicate code due to traits
    }
}
