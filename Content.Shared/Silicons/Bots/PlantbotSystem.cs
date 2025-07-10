// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Emag.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.Silicons.Bots;

/// <summary>
/// Handles emagging Plantbots
/// </summary>
public abstract class SharedPlantbotSystem : EntitySystem
{
    [Dependency] protected readonly SharedAudioSystem AudioSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlantbotComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnEmagged(EntityUid uid, PlantbotComponent comp, ref GotEmaggedEvent args)
    {
        AudioSystem.PlayPredicted(comp.EmagSparkSound, uid, args.UserUid);

        comp.IsEmagged = true;
        args.Handled = true;
    }
}
