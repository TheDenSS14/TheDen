// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Consent;
using Robust.Shared.Prototypes;


namespace Content.Client.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    public override bool HasConsent(EntityUid uid, ProtoId<ConsentTogglePrototype> consentId)
    {
        if (!TryComp<ConsentComponent>(uid, out var consent))
            return false;

        return consent.Consents.Any(targetConsent => targetConsent.Id == consentId);
    }
}
