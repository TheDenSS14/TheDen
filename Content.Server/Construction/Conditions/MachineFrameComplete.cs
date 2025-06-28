// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 E F R <602406+Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Construction.Components;
using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Shared.Utility;

namespace Content.Server.Construction.Conditions
{
    /// <summary>
    ///     Checks that the entity has all parts needed in the machine frame component.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class MachineFrameComplete : IGraphCondition
    {
        [DataField("guideIconBoard")]
        public SpriteSpecifier? GuideIconBoard { get; private set; }

        [DataField("guideIconParts")]
        public SpriteSpecifier? GuideIconPart { get; private set; }


        public bool Condition(EntityUid uid, IEntityManager entityManager)
        {
            if (!entityManager.TryGetComponent(uid, out MachineFrameComponent? machineFrame))
                return false;

            return entityManager.EntitySysManager.GetEntitySystem<MachineFrameSystem>().IsComplete(machineFrame);
        }

        public bool DoExamine(ExaminedEvent args)
        {
            var entity = args.Examined;

            var entityManager = IoCManager.Resolve<IEntityManager>();

            if (!entityManager.TryGetComponent(entity, out MachineFrameComponent? machineFrame))
                return false;

            if (!machineFrame.HasBoard)
            {
                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-insert-circuit-board-message"));
                return true;
            }

            if (entityManager.EntitySysManager.GetEntitySystem<MachineFrameSystem>().IsComplete(machineFrame))
                return false;

            args.PushMarkup(Loc.GetString("construction-condition-machine-frame-requirement-label"));
            foreach (var (part, required) in machineFrame.Requirements)
            {
                var amount = required - machineFrame.Progress[part];

                if(amount == 0)
                    continue;

                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                           ("amount", amount),
                                           ("elementName", Loc.GetString(part))));
            }

            foreach (var (material, required) in machineFrame.MaterialRequirements)
            {
                var amount = required - machineFrame.MaterialProgress[material];

                if(amount == 0)
                    continue;

                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                           ("amount", amount),
                                           ("elementName", Loc.GetString(material))));
            }

            foreach (var (compName, info) in machineFrame.ComponentRequirements)
            {
                var amount = info.Amount - machineFrame.ComponentProgress[compName];

                if(amount == 0)
                    continue;

                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                                ("amount", info.Amount),
                                                ("elementName", Loc.GetString(info.ExamineName))));
            }

            foreach (var (tagName, info) in machineFrame.TagRequirements)
            {
                var amount = info.Amount - machineFrame.TagProgress[tagName];

                if(amount == 0)
                    continue;

                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                    ("amount", info.Amount),
                                    ("elementName", Loc.GetString(info.ExamineName)))
                                + "\n");
            }

            return true;
        }

        public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
        {
            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-step-condition-machine-frame-board",
                Icon = GuideIconBoard,
                EntryNumber = 0, // Set this to anything so the guide generation takes this as a numbered step.
            };

            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-step-condition-machine-frame-parts",
                Icon = GuideIconPart,
                EntryNumber = 0, // Set this to anything so the guide generation takes this as a numbered step.
            };
        }
    }
}
