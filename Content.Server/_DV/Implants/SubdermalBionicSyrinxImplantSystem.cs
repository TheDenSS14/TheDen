// SPDX-FileCopyrightText: 2023 Bakke <luringens@protonmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 beck-thompson <107373427+beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.VoiceMask;
using Content.Shared.Implants;
using Content.Shared.Tag;

namespace Content.Server.Implants;

public sealed class SubdermalBionicSyrinxImplantSystem : EntitySystem
{
    [Dependency] private readonly TagSystem _tag = default!;

    [ValidatePrototypeId<TagPrototype>]
    private const string BionicSyrinxImplant = "BionicSyrinxImplant";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<VoiceMaskComponent, ImplantImplantedEvent>(OnInsert);
    }

    private void OnInsert(EntityUid uid, VoiceMaskComponent component, ImplantImplantedEvent args)
    {
        if (!args.Implanted.HasValue ||
            !_tag.HasTag(args.Implant, BionicSyrinxImplant))
            return;

        // Update the name so it's the entities default name. You can't take it off like a voice mask so it's important!
        component.VoiceMaskName = Name(args.Implanted.Value);
    }
}
