// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(PsionicCatGotYourTongueRule))]
public sealed partial class PsionicCatGotYourTongueRuleComponent : Component
{
    [DataField("minDuration")]
    public TimeSpan MinDuration = TimeSpan.FromSeconds(20);

    [DataField("maxDuration")]
    public TimeSpan MaxDuration = TimeSpan.FromSeconds(80);

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/Nyanotrasen/Voice/Felinid/cat_scream1.ogg");
}
