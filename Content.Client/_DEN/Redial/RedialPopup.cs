using Content.Client.UserInterface.Controls;
using Robust.Client;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._DEN.Redial;

public sealed class RedialPopup : FancyWindow
{
    [Dependency] private readonly IGameController _gameController = null!;
    [Dependency] private readonly ILogManager _logManager = null!;

    public Action<bool>? ButtonClicked;

    private readonly ISawmill _sawmill;
    private readonly BoxContainer _buttonContainer;

    public RedialPopup(string redialAddress, string redialMessage)
    {
        IoCManager.InjectDependencies(this);
        _sawmill = _logManager.GetSawmill("dialog.window");
        Title = Loc.GetString("redial-window-title");

        var container = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical
        };

        ContentsContainer.AddChild(container);

        var text = new RichTextLabel
        {
            Text = Loc.GetString("redial-window-text", ("message", redialMessage)),
            HorizontalExpand = true,
            Margin = new(5, 2, 5, 5)
        };

        container.AddChild(text);

        var separator = new Control
        {
            VerticalExpand = true
        };

        container.AddChild(separator);

        var buttonContainer = new BoxContainer
        {
            Margin = new(0, 7),
            Align = BoxContainer.AlignMode.Center
        };

        container.AddChild(buttonContainer);
        _buttonContainer = buttonContainer;

        MinHeight = 125;
        MinWidth = 125;

        var yesButton = AddButton("redial-button-yes");
        var dieForeverButton = AddButton("redial-button-dont-show");
        var noButton = AddButton("redial-button-no");

        yesButton.OnPressed += _ =>
        {
            TryConnect(redialAddress);
            Orphan();
            ButtonClicked?.Invoke(false);
        };

        dieForeverButton.OnPressed += _ =>
        {
            ButtonClicked?.Invoke(true);
        };

        noButton.OnPressed += _ =>
        {
            Orphan();
            ButtonClicked?.Invoke(false);
        };

        InvalidateMeasure();
    }

    private Button AddButton(LocId text, bool confirmButton = false)
    {
        var button = confirmButton ? new ConfirmButton() : new Button();
        button.Text = Loc.GetString(text);
        button.InvalidateMeasure();

        _buttonContainer.AddChild(button);
        InvalidateMeasure();

        return button;
    }

    private void TryConnect(string connectAddress)
    {
        try
        {
            _gameController.Redial(connectAddress);
        }
        catch (Exception ex)
        {
            _sawmill.Error($"{ex}");
        }
    }
}
