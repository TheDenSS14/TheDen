// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Psionics;

namespace Content.Server.Revolutionary.Components;
public sealed partial class CommandStaffSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CommandStaffComponent, OnRollPsionicsEvent>(OnRollPsionics);
    }

    private void OnRollPsionics(EntityUid uid, CommandStaffComponent component, ref OnRollPsionicsEvent args)
    {
        args.BaselineChance = args.BaselineChance * component.PsionicBonusModifier + component.PsionicBonusOffset;
    }
}