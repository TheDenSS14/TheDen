// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2025 Will Oliver <willyangame76@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Traits.Assorted.Components;
using Content.Shared._Shitmed.Body.Events;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;

namespace Content.Shared.Traits.Assorted.Systems;

public sealed class LegsStartParalyzedSystem : EntitySystem
{
    [Dependency] private readonly EntityManager _entMan = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<LegsStartParalyzedComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, LegsStartParalyzedComponent component, MapInitEvent args)
    {
        if (!_entMan.TryGetComponent<BodyComponent>(uid, out var body))
            return;

        foreach (var legEntity in body.LegEntities)
        {
            if (TryComp(legEntity, out BodyPartComponent? part))
            {
                part.CanEnable = false;
                Dirty(legEntity, part);
            }

            var ev = new BodyPartEnableChangedEvent(false);
            RaiseLocalEvent(legEntity, ref ev);
        }
    }
}
