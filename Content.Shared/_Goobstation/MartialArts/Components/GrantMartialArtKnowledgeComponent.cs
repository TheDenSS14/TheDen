// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.MartialArts.Components;

public abstract partial class GrantMartialArtKnowledgeComponent : Component
{
    [DataField]
    public bool Used;

    [DataField]
    public virtual MartialArtsForms MartialArtsForm { get; set; } = MartialArtsForms.CloseQuartersCombat;
}

[RegisterComponent]
public sealed partial class GrantCqcComponent : GrantMartialArtKnowledgeComponent
{
    [DataField]
    public bool IsBlocked;
}

[RegisterComponent]
public sealed partial class GrantCorporateJudoComponent : GrantMartialArtKnowledgeComponent
{
    [DataField]
    public override MartialArtsForms MartialArtsForm { get; set; } = MartialArtsForms.CorporateJudo;
}

[RegisterComponent]
public sealed partial class GrantSleepingCarpComponent : GrantMartialArtKnowledgeComponent
{
    [DataField]
    public override MartialArtsForms MartialArtsForm { get; set; } = MartialArtsForms.SleepingCarp;

    [DataField]
    public int Stage = 1;

    public TimeSpan UseAgainTime = TimeSpan.Zero;

    [DataField]
    public int MaxUseDelay = 90;

    [DataField]
    public int MinUseDelay = 30;
}
