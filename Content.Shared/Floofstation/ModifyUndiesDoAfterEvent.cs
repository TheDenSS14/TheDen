using Content.Shared.DoAfter;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Robust.Shared.Serialization;


namespace Content.Shared.FloofStation;


[Serializable, NetSerializable]
public sealed partial class ModifyUndiesDoAfterEvent : DoAfterEvent
{
    /// <summary>
    ///     The marking prototype that is being modified.
    /// </summary>
    [DataField("markingPrototype", required: true)]
    public Marking Marking;

    /// <summary>
    ///     Localized string for the marking prototype.
    /// </summary>
    [DataField("markingPrototypeName", required: true)]
    public string MarkingPrototypeName;

    /// <summary>
    ///     Whether or not the marking is visible at the moment.
    /// </summary>
    [DataField("visible", required: true)]
    public bool IsVisible;

    private ModifyUndiesDoAfterEvent()
    {
        Marking = default!;
        MarkingPrototypeName = string.Empty;
        IsVisible = false;
    }

    public ModifyUndiesDoAfterEvent(
        Marking marking,
        string markingPrototypeName,
        bool isVisible
        )
    {
        Marking = marking;
        MarkingPrototypeName = markingPrototypeName;
        IsVisible = isVisible;
    }

    public override DoAfterEvent Clone() => this;
}

