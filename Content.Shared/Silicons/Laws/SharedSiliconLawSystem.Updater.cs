// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Silicons.Laws.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Silicons.Laws;

public abstract partial class SharedSiliconLawSystem
{
    private void InitializeUpdater()
    {
        SubscribeLocalEvent<SiliconLawUpdaterComponent, EntInsertedIntoContainerMessage>(OnUpdaterInsert);
    }

    protected virtual void OnUpdaterInsert(Entity<SiliconLawUpdaterComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        // TODO: Prediction
    }
}
