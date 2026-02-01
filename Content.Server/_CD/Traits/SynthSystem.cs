// SPDX-FileCopyrightText: 2024 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Systems;
using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Speech.Components;

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
        _bloodstream.ChangeBloodReagent(uid, "SynthBlood");

        // Gives them the DamagedSiliconAccent component
        EnsureComp<DamagedSiliconAccentComponent>(uid, out var accent);
        accent.EnableChargeCorruption = false; //Disables corruption on low battery. This would always be active since non-silicons don't have a battery
        accent.DamageAtMaxCorruption = 200; //This is makes it usable for anyone not a silicon
        Dirty(uid, accent);
    }
}
