// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Server.GameObjects;
using Robust.Shared.Random;
using Content.Server.Abilities.Psionics;
using Content.Server.Consent;
using Content.Shared.GameTicking.Components;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Psionics;
using Content.Server.StationEvents.Components;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Consent;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;


namespace Content.Server.StationEvents.Events;

/// <summary>
/// Forces a mind swap on all non-insulated potential psionic entities.
/// </summary>
internal sealed class MassMindSwapRule : StationEventSystem<MassMindSwapRuleComponent>
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly MindSwapPowerSystem _mindSwap = default!;
    [Dependency] private readonly ConsentSystem _consent = default!;

    private static readonly ProtoId<ConsentTogglePrototype> MassMindSwapConsent = "MassMindSwap";

    protected override void Started(EntityUid uid, MassMindSwapRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        List<EntityUid> psionicPool = new();
        List<EntityUid> psionicActors = new();

        var query = EntityQueryEnumerator<PsionicComponent, MobStateComponent>();
        while (query.MoveNext(out var psion, out _, out _))
        {
            if (_mobStateSystem.IsAlive(psion)
                && !HasComp<PsionicInsulationComponent>(psion)
                && HasComp<MindContainerComponent>(psion)
                && _consent.HasConsent(psion, MassMindSwapConsent))
            {
                psionicPool.Add(psion);

                if (HasComp<ActorComponent>(psion))
                {
                    // This is so we don't bother mindswapping NPCs with NPCs.
                    psionicActors.Add(psion);
                }
            }
        }

        // Shuffle the list of candidates.
        _random.Shuffle(psionicPool);

        foreach (var actor in psionicActors)
        {
            do
            {
                if (psionicPool.Count == 0)
                    // We ran out of candidates. Exit early.
                    return;

                // Pop the last entry off.
                var other = psionicPool[^1];

                if (!HasComp<MindContainerComponent>(other)
                    || !_consent.HasConsent(other, MassMindSwapConsent))
                    continue;

                psionicPool.RemoveAt(psionicPool.Count - 1);

                if (other == actor)
                    // Don't be yourself. Find someone else.
                    continue;

                // A valid swap target has been found.
                // Remove this actor from the pool of swap candidates before they go.
                psionicPool.Remove(actor);

                // Do the swap.
                _mindSwap.Swap(actor, other);
                if (!component.IsTemporary)
                {
                    _mindSwap.GetTrapped(actor);
                    _mindSwap.GetTrapped(other);
                }
            } while (true);
        }
    }
}
