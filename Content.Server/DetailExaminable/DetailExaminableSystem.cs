// SPDX-FileCopyrightText: 2022 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Ygg01 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 MajorMoth <61519600+MajorMoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._Floof.Consent;
using Content.Shared._Floof.Consent;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.DetailExaminable
{
    public sealed class DetailExaminableSystem : EntitySystem
    {
        [Dependency] private readonly ExamineSystemShared _examineSystem = default!;
        [Dependency] private readonly ConsentSystem _consentSystem = default!;

        private ProtoId<ConsentTogglePrototype> _nsfwDescriptionsConsent = "NSFWDescriptions";

        // DEN - Icons
        private SpriteSpecifier _detailVerbIcon =
            new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/examine.svg.192dpi.png"));

        private SpriteSpecifier _lewdVerbIcon =
            new SpriteSpecifier.Texture(new("/Textures/_DEN/Interface/VerbIcons/lewd.svg.192dpi.png"));

        private SpriteSpecifier _selfVerbIcon =
            new SpriteSpecifier.Texture(new("/Textures/_DEN/Interface/VerbIcons/mirror.svg.192dpi.png"));
        // End DEN

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<DetailExaminableComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
        }

        private void OnGetExamineVerbs(EntityUid uid, DetailExaminableComponent component, GetVerbsEvent<ExamineVerb> args)
        {
            if (Identity.Name(args.Target, EntityManager) != MetaData(args.Target).EntityName)
                return;

            var contentVerb = GetContentExamine(uid, component, args);
            if (contentVerb != null) // DEN: Have to null-check becuase GetContentExamine is now nullable
                args.Verbs.Add(contentVerb);

            var nsfwContentVerb = GetNsfwContentExamine(uid, component, args);
            if (nsfwContentVerb != null)
                args.Verbs.Add(nsfwContentVerb);

            // DEN: Add self-examination
            var selfExamineVerb = GetSelfExamine(uid, component, args);
            if (selfExamineVerb != null)
                args.Verbs.Add(selfExamineVerb);
        }

        // DEN start: Common function for building examine verbs
        private ExamineVerb? GetExamineVerb(EntityUid uid,
            string content,
            string verbText,
            SpriteSpecifier? icon,
            GetVerbsEvent<ExamineVerb> args,
            ProtoId<ConsentTogglePrototype>? requiredConsent = null,
            bool hideIfEmpty = false)
        {
            if (hideIfEmpty && string.IsNullOrWhiteSpace(content)
                || requiredConsent is not null && !_consentSystem.HasConsent(args.User, requiredConsent.Value))
                return null;

            var verb = new ExamineVerb
            {
                Act = () =>
                {
                    var markup = new FormattedMessage();
                    markup.AddMarkupPermissive(content);
                    _examineSystem.SendExamineTooltip(args.User, uid, markup, getVerbs: false, centerAtCursor: false);
                },
                Text = verbText,
                Category = VerbCategory.Examine,
                Icon = icon
            };

            return verb;
        }
        // End DEN

        private ExamineVerb? GetContentExamine(
            EntityUid uid,
            DetailExaminableComponent component,
            GetVerbsEvent<ExamineVerb> args
        )
        {
            // DEN: Use shared detail examine system for this
            return GetExamineVerb(uid,
                content: component.Content,
                verbText: Loc.GetString("detail-examinable-verb-text"),
                icon: _detailVerbIcon,
                args: args,
                hideIfEmpty: false);
        }

        private ExamineVerb? GetNsfwContentExamine(
            EntityUid uid,
            DetailExaminableComponent component,
            GetVerbsEvent<ExamineVerb> args
        )
        {
            // DEN: Use shared detail examine system for this
            return GetExamineVerb(uid,
                content: component.NsfwContent,
                verbText: Loc.GetString("detail-nsfw-examinable-verb-text"),
                icon: _lewdVerbIcon,
                args: args,
                requiredConsent: _nsfwDescriptionsConsent,
                hideIfEmpty: true);
        }

        private ExamineVerb? GetSelfExamine(
            EntityUid uid,
            DetailExaminableComponent component,
            GetVerbsEvent<ExamineVerb> args)
        {
            if (args.User != args.Target)
                return null;

            return GetExamineVerb(uid,
                content: component.SelfContent,
                verbText: Loc.GetString("detail-self-examine-verb-text"),
                icon: _selfVerbIcon,
                args: args,
                hideIfEmpty: true);
        }
    }
}
