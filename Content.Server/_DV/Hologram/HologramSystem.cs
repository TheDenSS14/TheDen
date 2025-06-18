// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions.Events;
using Content.Shared._DV.Hologram;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Robust.Shared.Player;

namespace Content.Server._DV.Hologram;

public sealed class HologramSystem : SharedHologramSystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HologramComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<HologramComponent, DisarmAttemptEvent>(OnDisarmAttempt);
    }

    private void OnExamine(EntityUid uid, HologramComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("hologram-on-examine"));
    }

    private void OnDisarmAttempt(EntityUid uid, HologramComponent component, DisarmAttemptEvent args)
    {
        if (component.PreventDisarm)
        {
            args.Cancel();

            var filterOther = Filter.PvsExcept(args.DisarmerUid, entityManager: EntityManager);
            var messageUser = Loc.GetString("hologram-disarm-blocked",
                ("target", Identity.Entity(args.TargetUid, EntityManager)));
            var messageOther = Loc.GetString("hologram-disarm-blocked-other",
                ("target", Identity.Entity(args.TargetUid, EntityManager)), ("performerName", args.DisarmerUid));

            _popup.PopupEntity(messageOther, args.DisarmerUid, filterOther, true);
            _popup.PopupEntity(messageUser, args.TargetUid, args.DisarmerUid);
        }

    }
}
