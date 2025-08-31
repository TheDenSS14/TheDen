// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2024 Plykiya
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Chemistry.EntitySystems;

public sealed class HyposprayStatusControl : Control
{
    private readonly Entity<HyposprayComponent> _parent;
    private readonly RichTextLabel _label;
    private readonly SharedSolutionContainerSystem _solutionContainers;

    private FixedPoint2 _prevVolume;
    private FixedPoint2 _prevMaxVolume;
    private bool _prevOnlyAffectsMobs;

    public HyposprayStatusControl(Entity<HyposprayComponent> parent, SharedSolutionContainerSystem solutionContainers)
    {
        _parent = parent;
        _solutionContainers = solutionContainers;
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
        AddChild(_label);
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!_solutionContainers.TryGetSolution(_parent.Owner, _parent.Comp.SolutionName, out _, out var solution))
            return;

        // only updates the UI if any of the details are different than they previously were
        if (_prevVolume == solution.Volume
            && _prevMaxVolume == solution.MaxVolume
            && _prevOnlyAffectsMobs == _parent.Comp.OnlyAffectsMobs)
            return;

        _prevVolume = solution.Volume;
        _prevMaxVolume = solution.MaxVolume;
        _prevOnlyAffectsMobs = _parent.Comp.OnlyAffectsMobs;

        var modeStringLocalized = Loc.GetString(GetLocalizedMode(_parent.Comp));

        _label.SetMarkup(Loc.GetString("hypospray-volume-label",
            ("currentVolume", solution.Volume),
            ("totalVolume", solution.MaxVolume),
            ("modeString", modeStringLocalized)));
    }

    private string GetLocalizedMode(HyposprayComponent comp)
    {
        if (comp.InjectOnly)
            return "hypospray-inject-only-mode-text";

        return comp.OnlyAffectsMobs switch
        {
            false => "hypospray-all-mode-text",
            true => "hypospray-mobs-only-mode-text",
        };
    }
}
