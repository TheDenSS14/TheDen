// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Forensics;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Client.Player;

namespace Content.Client.Forensics
{
    public sealed class ScentTrackerSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly SharedTransformSystem _transform = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            var query = AllEntityQuery<ForensicsComponent>();
            while (query.MoveNext(out var uid, out var comp))
                if (TryComp<ScentTrackerComponent>(_playerManager.LocalEntity, out var scentcomp)
                    && scentcomp.Scent != string.Empty
                    && scentcomp.Scent == comp.Scent
                    && _timing.CurTime > comp.TargetTime)
                {
                    comp.TargetTime = _timing.CurTime + TimeSpan.FromSeconds(1.0f);
                    Spawn("ScentTrackEffect", _transform.GetMapCoordinates(uid).Offset(_random.NextVector2(0.25f)));
                }
        }
    }
}