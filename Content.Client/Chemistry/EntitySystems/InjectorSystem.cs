// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Plykiya <58439124+Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chemistry.UI;
using Content.Client.Items;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Client.Chemistry.EntitySystems;

public sealed class InjectorSystem : SharedInjectorSystem
{
    public override void Initialize()
    {
        base.Initialize();
        Subs.ItemStatus<InjectorComponent>(ent => new InjectorStatusControl(ent, SolutionContainers));
    }
}
