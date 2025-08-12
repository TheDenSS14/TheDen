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

        // DEN - Icon
        private SpriteSpecifier _detailVerbIcon =
            new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/examine.svg.192dpi.png"));

        private SpriteSpecifier _lewdVerbIcon =
            new SpriteSpecifier.Texture(new("/Textures/_DEN/Interface/VerbIcons/lewd.svg.192dpi.png"));


        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<DetailExaminableComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
        }

        private void OnGetExamineVerbs(EntityUid uid, DetailExaminableComponent component, GetVerbsEvent<ExamineVerb> args)
        {
            if (Identity.Name(args.Target, EntityManager) != MetaData(args.Target).EntityName)
                return;

            // var detailsRange = _examineSystem.IsInDetailsRange(args.User, uid);

            var contentVerb = GetContentExamine(uid, component, args);
            var nsfwContentVerb = GetNsfwContentExamine(uid, component, args);
            args.Verbs.Add(contentVerb);

            if (nsfwContentVerb != null)
                args.Verbs.Add(nsfwContentVerb);
        }

        private ExamineVerb GetContentExamine(
            EntityUid uid,
            DetailExaminableComponent component,
            GetVerbsEvent<ExamineVerb> args
        )
        {
            var detailsRange = true; //removed the range limitation due to player requests, the detail examine button should now be active all the time
            var verb = new ExamineVerb
            {
                Act = () =>
                {
                    var markup = new FormattedMessage();
                    markup.AddMarkupPermissive(component.Content);
                    _examineSystem.SendExamineTooltip(args.User, uid, markup, false, false);
                },
                Text = Loc.GetString("detail-examinable-verb-text"),
                Category = VerbCategory.Examine,
                Disabled = !detailsRange,
                Message = detailsRange ? null : Loc.GetString("detail-examinable-verb-disabled"),
                Icon = _detailVerbIcon
            };

            return verb;
        }

        private ExamineVerb? GetNsfwContentExamine(
            EntityUid uid,
            DetailExaminableComponent component,
            GetVerbsEvent<ExamineVerb> args
        )
        {
            if (!_consentSystem.HasConsent(args.User, _nsfwDescriptionsConsent)
                || string.IsNullOrWhiteSpace(component.NsfwContent))
                return null;

            var detailsRange = true;
            var verb = new ExamineVerb
            {
                Act = () =>
                {
                    var markup = new FormattedMessage();
                    markup.AddMarkupPermissive(component.NsfwContent);
                    _examineSystem.SendExamineTooltip(args.User, uid, markup, false, false);
                },
                Text = Loc.GetString("detail-nsfw-examinable-verb-text"),
                Category = VerbCategory.Examine,
                Disabled = !detailsRange,
                Message = detailsRange ? null : Loc.GetString("detail-examinable-verb-disabled"),
                Icon = _lewdVerbIcon
            };

            return verb;
        }
    }
}
