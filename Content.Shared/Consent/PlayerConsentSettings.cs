// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Consent;

[Serializable, NetSerializable]
public sealed class PlayerConsentSettings
{
    public string Freetext;
    public Dictionary<ProtoId<ConsentTogglePrototype>, bool> Toggles;

    public PlayerConsentSettings()
    {
        Freetext = string.Empty;
        Toggles = new();
    }

    public PlayerConsentSettings(
        string freetext,
        Dictionary<ProtoId<ConsentTogglePrototype>, bool> toggles)
    {
        Freetext = freetext.Trim();
        Toggles = toggles;
    }

    public void EnsureValid(IConfigurationManager configManager, IPrototypeManager prototypeManager)
    {
        var maxLength = configManager.GetCVar(CCVars.ConsentFreetextMaxLength);

        if (Freetext.Length > maxLength)
            Freetext = Freetext.Substring(0, maxLength);

        Toggles = Toggles.Where(t =>
            prototypeManager.HasIndex<ConsentTogglePrototype>(t.Key)
            && t.Value
        )
            .ToDictionary();
    }
}
