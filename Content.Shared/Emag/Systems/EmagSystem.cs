// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 Arendian
// SPDX-FileCopyrightText: 2024 WarMechanic
// SPDX-FileCopyrightText: 2025 Shaman
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Impstation.Thaven.Components; // imp
using Content.Shared.Administration.Logs;
using Content.Shared.Bed.Sleep; // imp
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Database;
using Content.Shared.Emag.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Systems; // imp
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
    [Dependency] private readonly MobStateSystem _mobState = default!; // imp - thaven can only be emagged when crit or dead

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

        // Imp start - if the target is a thaven who is not sleeping, dead, or crit, skip
        if (TryComp<ThavenMoodsComponent>(target, out _) && !HasComp<SleepingComponent>(target) && !_mobState.IsIncapacitated(target) && target != user)
        {
            _popup.PopupClient(Loc.GetString("emag-thaven-alive", ("emag", ent), ("target", target)), user, user);
            return false;
        }
        // Imp end // TODO (THAVEN): THIS ALSO NEEDS TO CHECK FOR A CONSENT SETTING SOMEHOW, MAYBE USING THE HYPNOSIS FLAG OR SOMETHING?  SHOULD IT CHECK FOR CONSENT IF A PLAYER DOES IT TO THEMSELVES WHILE AWAKE?  MAYBE THAT SHOULD HAVE A SUICIDE-LIKE "YOU SURE YOU WANNA??" CHECK?  IT NEEDS SOMETHING FOR PLAYER COMFORT.
    }
}

[ByRefEvent]
public record struct GotEmaggedEvent(EntityUid UserUid, bool Handled = false, bool Repeatable = false);

[ByRefEvent]
public record struct OnAttemptEmagEvent(EntityUid UserUid, bool Handled = false);
