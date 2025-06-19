// SPDX-FileCopyrightText: 2024 WarMechanic <69510347+WarMechanic@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Traits.Assorted.Components;

namespace Content.Shared.Traits.Assorted.Systems;
public sealed class LightweightDrunkSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<LightweightDrunkComponent, TryMetabolizeReagent>(OnTryMetabolizeReagent);
    }

    private void OnTryMetabolizeReagent(EntityUid uid, LightweightDrunkComponent comp, ref TryMetabolizeReagent args)
    {
        //Log.Debug(args.Prototype.ID);
        if (args.Prototype.ID != "Ethanol")
            return;

        args.Scale *= comp.BoozeStrengthMultiplier;
        args.QuantityMultiplier *= comp.BoozeStrengthMultiplier;
    }
}