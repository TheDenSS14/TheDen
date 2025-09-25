// SPDX-FileCopyrightText: 2025 Shaman
// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Emag.Systems;
using Content.Shared._Impstation.Thaven.Components;

namespace Content.Shared._Impstation.Thaven;

public abstract partial class SharedThavenMoodSystem : EntitySystem
{
    [Dependency] private readonly EmagSystem _emag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThavenMoodsComponent, GotEmaggedEvent>(OnEmagged);
        SubscribeLocalEvent<ThavenMoodsComponent, OnAttemptEmagEvent>(OnAttemptEmag);
    }

    protected virtual void OnEmagged(Entity<ThavenMoodsComponent> ent, ref GotEmaggedEvent args)
    {
        args.Handled = true;
    }
}
