// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Psionics;

namespace Content.Server.Traits.Assorted;
public sealed partial class PotentiaModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PotentiaModifierComponent, OnRollPsionicsEvent>(OnRollPsionics);
    }

    private void OnRollPsionics(EntityUid uid, PotentiaModifierComponent component, ref OnRollPsionicsEvent args)
    {
        if (uid != args.Roller)
            return;

        args.BaselineChance = args.BaselineChance * component.PotentiaMultiplier + component.PotentiaFlatModifier;
    }
}
