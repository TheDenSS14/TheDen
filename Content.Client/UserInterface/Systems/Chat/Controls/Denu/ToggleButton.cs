using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public class ToggleButton : Button
{
    public long UpdatePeriod { get; set; } = 1000;
    public Action OnToggledOn { get; set; } = () => { };
    public Action OnToggledOff { get; set; } = () => { };
    public Action WhileToggled { get; set; } = () => { };

    long _lastUpdate = 0;

    public ToggleButton()
    {
        ToggleMode = true;
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

