// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Shared.Remotes.Components;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Remote.UI;

public sealed class DoorRemoteStatusControl : Control
{
    private readonly Entity<DoorRemoteComponent> _entity;
    private readonly RichTextLabel _label;

    // set to toggle bolts initially just so that it updates on first pickup of remote
    private OperatingMode PrevOperatingMode = OperatingMode.placeholderForUiUpdates;

    public DoorRemoteStatusControl(Entity<DoorRemoteComponent> entity)
    {
        _entity = entity;
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
        AddChild(_label);
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        // only updates the UI if any of the details are different than they previously were
        if (PrevOperatingMode == _entity.Comp.Mode)
            return;

        PrevOperatingMode = _entity.Comp.Mode;

        // Update current volume and injector state
        var modeStringLocalized = Loc.GetString(_entity.Comp.Mode switch
        {
            OperatingMode.OpenClose => "door-remote-open-close-text",
            OperatingMode.ToggleBolts => "door-remote-toggle-bolt-text",
            OperatingMode.ToggleEmergencyAccess => "door-remote-emergency-access-text",
            _ => "door-remote-invalid-text"
        });

        _label.SetMarkup(Loc.GetString("door-remote-mode-label", ("modeString", modeStringLocalized)));
    }
}
