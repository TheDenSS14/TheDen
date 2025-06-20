// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Nutrition.Components;
using Content.Shared.Clothing;
using Content.Shared.Examine;

namespace Content.Server.Nutrition.EntitySystems;

public sealed class IngestionBlockerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IngestionBlockerComponent, ItemMaskToggledEvent>(OnBlockerMaskToggled);

        SubscribeLocalEvent<IngestionBlockerComponent, ExaminedEvent>(OnExamined); // Goobstation
    }

    private void OnExamined(Entity<IngestionBlockerComponent> ent, ref ExaminedEvent args) // Goobstation
    {
        if (ent.Comp.BlockSmokeIngestion)
            args.PushMarkup(Loc.GetString("ingestion-blocker-block-smoke-examine"));
    }

    private void OnBlockerMaskToggled(Entity<IngestionBlockerComponent> ent, ref ItemMaskToggledEvent args)
    {
        ent.Comp.Enabled = !args.IsToggled;
    }
}
