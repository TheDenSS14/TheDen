// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Arendian <137322659+Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 WarMechanic <69510347+WarMechanic@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Administration.Logs;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Database;
using Content.Shared.Emag.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Tag;

namespace Content.Shared.Emag.Systems;

/// How to add an emag interaction:
/// 1. Go to the system for the component you want the interaction with
/// 2. Subscribe to the GotEmaggedEvent
/// 3. Have some check for if this actually needs to be emagged or is already emagged (to stop charge waste)
/// 4. Past the check, add all the effects you desire and HANDLE THE EVENT ARGUMENT so a charge is spent
/// 5. Optionally, set Repeatable on the event to true if you don't want the emagged component to be added
public sealed class EmagSystem : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedChargesSystem _charges = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EmagComponent, AfterInteractEvent>(OnAfterInteract);
    }

    private void OnAfterInteract(EntityUid uid, EmagComponent comp, AfterInteractEvent args)
    {
        if (!args.CanReach || args.Target is not { } target)
            return;

        args.Handled = TryUseEmag(uid, args.User, target, comp);
    }

    /// <summary>
    /// Tries to use the emag on a target entity
    /// </summary>
    public bool TryUseEmag(EntityUid uid, EntityUid user, EntityUid target, EmagComponent? comp = null)
    {
        if (!Resolve(uid, ref comp, false))
            return false;

        if (_tag.HasTag(target, comp.EmagImmuneTag))
            return false;

        TryComp<LimitedChargesComponent>(uid, out var charges);
        if (_charges.IsEmpty(uid, charges))
        {
            _popup.PopupClient(Loc.GetString("emag-no-charges"), user, user);
            return false;
        }

        var handled = DoEmagEffect(user, target);
        if (!handled)
            return false;

        _popup.PopupClient(Loc.GetString("emag-success", ("target", Identity.Entity(target, EntityManager))), user,
            user, PopupType.Medium);

        _adminLogger.Add(LogType.Emag, LogImpact.High, $"{ToPrettyString(user):player} emagged {ToPrettyString(target):target}");

        if (charges != null)
            _charges.UseCharge(uid, charges);
        return true;
    }

    /// <summary>
    /// Does the emag effect on a specified entity
    /// </summary>
    public bool DoEmagEffect(EntityUid user, EntityUid target)
    {
        // prevent emagging twice
        if (HasComp<EmaggedComponent>(target))
            return false;

        var onAttemptEmagEvent = new OnAttemptEmagEvent(user);
        RaiseLocalEvent(target, ref onAttemptEmagEvent);

        // prevent emagging if attempt fails
        if (onAttemptEmagEvent.Handled)
            return false;

        var emaggedEvent = new GotEmaggedEvent(user);
        RaiseLocalEvent(target, ref emaggedEvent);

        if (emaggedEvent.Handled && !emaggedEvent.Repeatable)
            EnsureComp<EmaggedComponent>(target);
        return emaggedEvent.Handled;
    }
}

[ByRefEvent]
public record struct GotEmaggedEvent(EntityUid UserUid, bool Handled = false, bool Repeatable = false);

[ByRefEvent]
public record struct OnAttemptEmagEvent(EntityUid UserUid, bool Handled = false);
