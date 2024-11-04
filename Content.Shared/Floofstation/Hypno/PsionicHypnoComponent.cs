using Content.Shared.DoAfter;
using Robust.Shared.Serialization;
using Robust.Shared.GameStates;

namespace Content.Shared.Floofstation.Hypno;

[RegisterComponent, NetworkedComponent]

public sealed partial class PsionicHypnoComponent : Component
{
    [DataField]
    public float UseDelay = 10f;

    [DataField]
    public DoAfterId? DoAfter;

    [DataField]
    public string MasterIcon = "HypnoMaster";

    [DataField]
    public string SubjectIcon = "HypnoSubject";
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
