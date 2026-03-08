// SPDX-FileCopyrightText: 2024 gluesniffler
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Asa
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Kitchen.Components;
using Content.Shared._Shitmed.Medical.Surgery.Tools;

namespace Content.Server._Shitmed.Medical.Surgery;

/// <summary>
/// Makes all sharp things usable for incisions and sawing through bones, though worse than any other kind of ghetto analogue.
/// </summary>
public sealed partial class GhettoSurgerySystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SharpComponent, MapInitEvent>(OnSharpInit);
        SubscribeLocalEvent<SharpComponent, ComponentShutdown>(OnSharpShutdown);
    }

    private void OnSharpInit(Entity<SharpComponent> ent, ref MapInitEvent args)
    {
        if (EnsureComp<ScalpelComponent>(ent, out var scalpel))
        {
            ent.Comp.HadScalpel = true;
        }
        else
        {
            scalpel.Speed = 0.3f;
            Dirty(ent.Owner, scalpel);
        }

        if (EnsureComp<BoneSawComponent>(ent, out var saw))
        {
            ent.Comp.HadBoneSaw = true;
        }
        else
        {
            saw.Speed = 0.2f;
            Dirty(ent.Owner, saw);
        }
    }

    private void OnSharpShutdown(Entity<SharpComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.HadScalpel)
            RemComp<ScalpelComponent>(ent);

        if (ent.Comp.HadBoneSaw)
            RemComp<BoneSawComponent>(ent);
    }
}
