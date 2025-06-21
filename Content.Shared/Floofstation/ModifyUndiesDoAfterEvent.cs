// SPDX-FileCopyrightText: 2025 Aikakakah <145503852+Aikakakah@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

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

