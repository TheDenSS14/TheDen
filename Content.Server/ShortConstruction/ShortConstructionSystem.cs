// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.RadialSelector;
using Content.Shared.ShortConstruction;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;

namespace Content.Server.ShortConstruction;

public sealed class ShortConstructionSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShortConstructionComponent, BeforeActivatableUIOpenEvent>(BeforeUiOpen);
    }

    private void BeforeUiOpen(Entity<ShortConstructionComponent> ent, ref BeforeActivatableUIOpenEvent args)
    {
        var state = new RadialSelectorState(ent.Comp.Entries);
        _ui.SetUiState(ent.Owner, RadialSelectorUiKey.Key, state);
    }
}
