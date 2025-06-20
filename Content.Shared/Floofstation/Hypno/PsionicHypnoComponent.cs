// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Content.Shared.StatusIcon;
using Robust.Shared.Serialization;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;


namespace Content.Shared.Floofstation.Hypno;

[RegisterComponent, NetworkedComponent]

public sealed partial class PsionicHypnoComponent : Component
{
    [DataField]
    public float UseDelay = 10f;

    [DataField]
    public DoAfterId? DoAfter;

    [DataField]
    public ProtoId<FactionIconPrototype> MasterIcon = "HypnoMaster";

    [DataField]
    public ProtoId<FactionIconPrototype> SubjectIcon = "HypnoSubject";
}

[Serializable, NetSerializable]
public sealed partial class PsionicHypnosisDoAfterEvent : DoAfterEvent
{
    [DataField("phase", required: true)]
    public int Phase;

    public PsionicHypnosisDoAfterEvent(int phase)
    {
        Phase = phase;
    }

    public override DoAfterEvent Clone() => this;
}
