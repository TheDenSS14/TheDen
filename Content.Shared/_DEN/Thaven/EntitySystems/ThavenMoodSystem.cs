// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Floof.Consent;
using Content.Shared._Impstation.Thaven.Components;
using Content.Shared.Bed.Sleep;
using Content.Shared.Emag.Systems;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Thaven;

public abstract partial class SharedThavenMoodSystem
{
    [Dependency] protected readonly SharedConsentSystem Consent = default!;
    [Dependency] protected readonly SharedPopupSystem Popup = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    protected readonly ProtoId<ConsentTogglePrototype> EmagConsentToggle = "AllowMoodEmagging";

    protected virtual void OnAttemptEmag(Entity<ThavenMoodsComponent> ent, ref OnAttemptEmagEvent args)
    {
        var user = args.UserUid;

        // Always allowed
        if (user == ent.Owner)
            return;

        // Thaven must be consenting!
        if (!Consent.HasConsent(ent.Owner, EmagConsentToggle))
        {
            Popup.PopupClient(Loc.GetString("emag-thaven-no-consent"), user, user, PopupType.MediumCaution);
            args.Handled = true;
            return;
        }

        // Thaven must be Not Awake
        if (!HasComp<SleepingComponent>(ent) && !_mobState.IsIncapacitated(ent))
        {
            Popup.PopupClient(Loc.GetString("emag-thaven-alive"), user, user, PopupType.MediumCaution);
            args.Handled = true;
        }
    }
}
