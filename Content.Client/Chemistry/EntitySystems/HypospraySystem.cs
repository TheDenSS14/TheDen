// SPDX-FileCopyrightText: 2024 Plykiya <58439124+Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chemistry.UI;
using Content.Client.Items;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;

namespace Content.Client.Chemistry.EntitySystems;

public sealed class HypospraySystem : SharedHypospraySystem
{
    public override void Initialize()
    {
        base.Initialize();
        Subs.ItemStatus<HyposprayComponent>(ent => new HyposprayStatusControl(ent, _solutionContainers));
    }
}
