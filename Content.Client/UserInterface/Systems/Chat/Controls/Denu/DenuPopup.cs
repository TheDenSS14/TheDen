using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;

public sealed partial class DenuPopup : Popup
{

    private readonly DenuUIController _denuUiController;

    public DenuPopup()
    {
        _denuUiController = UserInterfaceManager.GetUIController<DenuUIController>();
        AddChild(InitializeMenu());
    }

    public Control InitializeMenu()
    {
        return new PanelContainer().Apply(panelContainer => {
            panelContainer.StyleClasses.Add("BorderedWindowPanel");

            panelContainer.AddChild(new BoxContainer().Apply(boxContainer => {
                boxContainer.Orientation = BoxContainer.LayoutOrientation.Vertical;
                boxContainer.SeparationOverride = 8;
                boxContainer.Margin = new Thickness(8);

                boxContainer.AddChild(new FunctionalButton().Apply(button => {
                    button.Text = "Typing Toggle";
                    button.ToggleMode = true;
                    button.WhileToggled = () => _denuUiController.TypingToggleUpdate();
                    button.OnToggledOff = () => _denuUiController.TypingToggleDisabled();
                }));
            }));
        });
    }
}

public class FunctionalButton : Button
{
    public long UpdatePeriod { get; set; } = 1000;
    public Action OnToggledOn { get; set; } = () => { };
    public Action OnToggledOff { get; set; } = () => { };
    public Action WhileToggled { get; set; } = () => { };

    long _lastUpdate = 0;

    public FunctionalButton()
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

public static class Extensions
{
    public static T Apply<T>(this T target, Action<T> action)
    {
        action(target);
        return target;
    }
}