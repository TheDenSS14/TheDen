// SPDX-FileCopyrightText: 2024 Pierson Arnold <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Floof.Consent;


namespace Content.Client._Floof.Consent;

public interface IClientConsentManager
{
    event Action OnServerDataLoaded;
    bool HasLoaded { get; }

    void Initialize();
    void UpdateConsent(PlayerConsentSettings consentSettings);
    PlayerConsentSettings GetConsent();
}
