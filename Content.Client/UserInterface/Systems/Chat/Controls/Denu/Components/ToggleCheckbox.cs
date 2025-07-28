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

    private IGameTiming _gameTiming = default!;

    private double _lastUpdate = 0;

    public ToggleCheckbox()
    {
        OnToggled += e => OnToggleChanged(e.Pressed);
        _gameTiming = IoCManager.Resolve<IGameTiming>();
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

        var currentTime = _gameTiming.RealTime.TotalMilliseconds;
        if (_lastUpdate + UpdatePeriod > currentTime)
            return;

        _lastUpdate = currentTime;
        WhileToggled.Invoke();
    }
}
