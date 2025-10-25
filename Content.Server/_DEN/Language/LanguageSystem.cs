// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Language;
using Robust.Shared.Prototypes;

namespace Content.Server.Language;

public sealed partial class LanguageSystem
{
    // i hope our last few remaining friends give up on trying to save us
    public void ReplaceLanguagesUnderstood(Entity<LanguageKnowledgeComponent?> ent,
        List<ProtoId<LanguagePrototype>> languages)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.UnderstoodLanguages = languages;
        UpdateEntityLanguages(ent.Owner);
    }

    public void ReplaceLanguagesSpoken(Entity<LanguageKnowledgeComponent?> ent,
        List<ProtoId<LanguagePrototype>> languages)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.SpokenLanguages = languages;
        UpdateEntityLanguages(ent.Owner);
    }
}
