// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 fox
// SPDX-FileCopyrightText: 2025 Carlen White
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Language;
using Content.Shared.Language.Components;
using Content.Shared.Language.Systems;
using Content.Shared.PowerCell;
using Content.Shared.Language.Components.Translators;
using Content.Shared.Language.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.PowerCell.Components;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Language;

public sealed class TranslatorSystem : SharedTranslatorSystem
{
    [Dependency] private readonly SharedContainerSystem _containers = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly LanguageSystem _language = default!;
    [Dependency] private readonly ItemToggleSystem _toggle = default!;
    [Dependency] private readonly PowerCellSystem _powerCell = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IntrinsicTranslatorComponent, DetermineEntityLanguagesEvent>(OnDetermineLanguages);
        SubscribeLocalEvent<HoldsTranslatorComponent, DetermineEntityLanguagesEvent>(OnProxyDetermineLanguages);

        SubscribeLocalEvent<HandheldTranslatorComponent, EntGotInsertedIntoContainerMessage>(OnTranslatorInserted);
        SubscribeLocalEvent<HandheldTranslatorComponent, EntParentChangedMessage>(OnTranslatorParentChanged);
        SubscribeLocalEvent<HandheldTranslatorComponent, ItemToggledEvent>(OnItemToggled);

        SubscribeLocalEvent<HandheldTranslatorComponent, PowerCellChangedEvent>(OnPowerCellChanged);
    }

    private void OnDetermineLanguages(EntityUid uid, IntrinsicTranslatorComponent component, DetermineEntityLanguagesEvent ev)
    {
        if (!component.Enabled
            || component.LifeStage >= ComponentLifeStage.Removing
            || !TryComp<LanguageKnowledgeComponent>(uid, out var knowledge)
            || !_powerCell.HasActivatableCharge(uid))
            return;

        CopyLanguages(component, ev, knowledge);
    }

    private void OnProxyDetermineLanguages(EntityUid uid, HoldsTranslatorComponent component, DetermineEntityLanguagesEvent ev)
    {
        if (!TryComp<LanguageKnowledgeComponent>(uid, out var knowledge))
            return;

        foreach (var (translator, translatorComp) in component.Translators.ToArray())
        {
            if (!translatorComp.Enabled || !_powerCell.HasActivatableCharge(uid))
                continue;

            if (!_containers.TryGetContainingContainer(translator, out var container) || container.Owner != uid)
            {
                component.Translators.RemoveWhere(it => it.Owner == translator);
                continue;
            }

            CopyLanguages(translatorComp, ev, knowledge);
        }
    }

    private void OnTranslatorInserted(EntityUid translator, HandheldTranslatorComponent component, EntGotInsertedIntoContainerMessage args)
    {
        if (args.Container.Owner is not { Valid: true } holder || !HasComp<LanguageSpeakerComponent>(holder))
            return;

        var intrinsic = EnsureComp<HoldsTranslatorComponent>(holder);
        intrinsic.Translators.Add((translator, component));

        _language.UpdateEntityLanguages(holder);
    }

    private void OnTranslatorParentChanged(EntityUid translator, HandheldTranslatorComponent component, EntParentChangedMessage args)
    {
        if (!HasComp<HoldsTranslatorComponent>(args.OldParent))
            return;

        // Update the translator on the next tick - this is necessary because there's a good chance the removal from a container
        // Was caused by the player moving the translator within their inventory rather than removing it.
        // If that is not the case, then OnProxyDetermineLanguages will remove this translator from HoldsTranslatorComponent.Translators.
        Timer.Spawn(0, () =>
        {
            if (Exists(args.OldParent) && HasComp<LanguageSpeakerComponent>(args.OldParent))
                _language.UpdateEntityLanguages(args.OldParent.Value);
        });
    }

    private void OnItemToggled(EntityUid translator, HandheldTranslatorComponent component, ItemToggledEvent args)
    {
        component.Enabled = args.Activated;
        OnAppearanceChange(translator, component);

        if (_containers.TryGetContainingContainer((translator, null, null), out var holderCont)
            && HasComp<LanguageSpeakerComponent>(holderCont.Owner))
        {
            _language.UpdateEntityLanguages(holderCont.Owner);
        }

        if (args.User != null)
        {
            var loc = component.Enabled ? "translator-component-turnon" : "translator-component-shutoff";
            var message = Loc.GetString(loc, ("translator", translator));
            _popup.PopupEntity(message, translator, args.User.Value);
        }
    }

    private void OnPowerCellChanged(EntityUid translator, HandheldTranslatorComponent component, PowerCellChangedEvent args)
    {
        if (!args.Ejected)
            _toggle.TryActivate(translator);
    }

    private void CopyLanguages(BaseTranslatorComponent from, DetermineEntityLanguagesEvent to, LanguageKnowledgeComponent knowledge)
    {
        var addSpoken = CheckLanguagesMatch(from.RequiredLanguages, knowledge.SpokenLanguages, from.RequiresAllLanguages);
        var addUnderstood = CheckLanguagesMatch(from.RequiredLanguages, knowledge.UnderstoodLanguages, from.RequiresAllLanguages);

        if (addSpoken)
            foreach (var language in from.SpokenLanguages)
                to.SpokenLanguages.Add(language);

        if (addUnderstood)
            foreach (var language in from.UnderstoodLanguages)
                to.UnderstoodLanguages.Add(language);
    }

    /// <summary>
    ///     Checks whether any OR all required languages are provided. Used for utility purposes.
    /// </summary>
    public static bool CheckLanguagesMatch(ICollection<ProtoId<LanguagePrototype>> required, ICollection<ProtoId<LanguagePrototype>> provided, bool requireAll)
    {
        if (required.Count == 0)
            return true;

        return requireAll
            ? required.All(provided.Contains)
            : required.Any(provided.Contains);
    }
}
