// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Labels.UI;
using Content.Shared.Labels;
using Content.Shared.Labels.Components;
using Content.Shared.Labels.EntitySystems;

namespace Content.Client.Labels.EntitySystems;

public sealed class HandLabelerSystem : SharedHandLabelerSystem
{
    protected override void UpdateUI(Entity<HandLabelerComponent> ent)
    {
        if (UserInterfaceSystem.TryGetOpenUi(ent.Owner, HandLabelerUiKey.Key, out var bui)
            && bui is HandLabelerBoundUserInterface cBui)
        {
            cBui.Reload();
        }
    }
}
