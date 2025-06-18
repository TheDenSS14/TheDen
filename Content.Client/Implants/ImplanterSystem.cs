// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Implants.UI;
using Content.Client.Items;
using Content.Shared.Implants;
using Content.Shared.Implants.Components;

namespace Content.Client.Implants;

public sealed class ImplanterSystem : SharedImplanterSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ImplanterComponent, AfterAutoHandleStateEvent>(OnHandleImplanterState);
        Subs.ItemStatus<ImplanterComponent>(ent => new ImplanterStatusControl(ent));
    }

    private void OnHandleImplanterState(EntityUid uid, ImplanterComponent component, ref AfterAutoHandleStateEvent args)
    {
        component.UiUpdateNeeded = true;
    }
}
