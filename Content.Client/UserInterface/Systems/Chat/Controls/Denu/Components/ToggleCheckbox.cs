// SPDX-FileCopyrightText: 2025 Cam
// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu.Components;


public class ToggleCheckbox : CheckBox
{
    public long UpdatePeriod { get; set; } = 1000;
    public Action OnToggledOn { get; set; } = () => { };
    public Action OnToggledOff { get; set; } = () => { };
    public Action WhileToggled { get; set; } = () => { };

    long _lastUpdate = 0;

    public ToggleCheckbox()
    {
        OnToggled += e => OnToggleChanged(e.Pressed);
    }

    private void OnToggleChanged(bool pressed)
    {
        if (pressed)
            OnToggledOn.Invoke();
        else
            OnToggledOff.Invoke();
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!Pressed)
            return;

        var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (_lastUpdate + UpdatePeriod > currentTime)
            return;

        _lastUpdate = currentTime;
        WhileToggled.Invoke();
    }
}

