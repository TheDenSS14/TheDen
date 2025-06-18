// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Emag.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.Silicons.Bots;

/// <summary>
/// Handles emagging Weldbots
/// </summary>
public sealed class WeldbotSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WeldbotComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnEmagged(EntityUid uid, WeldbotComponent comp, ref GotEmaggedEvent args)
    {
        _audio.PlayPredicted(comp.EmagSparkSound, uid, args.UserUid);

        comp.IsEmagged = true;
        args.Handled = true;
    }
}
