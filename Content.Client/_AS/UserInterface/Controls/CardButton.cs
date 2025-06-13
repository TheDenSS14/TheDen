using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._AS.UserInterface.Controls;

/// <summary>
/// Used for consent cards.
/// </summary>
public sealed class CardButton : ContainerButton
{
    // Default style constants
    private const float DefaultImageScale = 1.5f;
    private const int LineSpacing = 3;

    // Private UI elements
    private readonly StyleBoxFlat _styleBox;
    private TextureRect? _image;
    private BoxContainer? _textContainer;
    private List<Label> _textLines = new();

    // Public properties
    public Texture? Icon
    {
        get => _image?.Texture;
        set
        {
            if (_image != null)
                _image.Texture = value;
        }
    }

    public string Description
    {
        get => GetDescriptionText();
        set => SetDescriptionText(value);
    }

    public Color BackgroundColor
    {
        get => _styleBox.BackgroundColor;
        set => _styleBox.BackgroundColor = value;
    }

    public Color HoverColor { get; set; } = Color.Transparent;
    public Color PressedColor { get; set; } = Color.Transparent;

    public CardButton()
    {
        // Initialize style box first
        _styleBox = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#2D2D38")
        };
        StyleBoxOverride = _styleBox;

        // Create UI elements
        CreateUiElements();
    }

    private void CreateUiElements()
    {
        // Main container (vertical layout)
        var container = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            VerticalExpand = true,
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            Margin = new Thickness(0)
        };

        // Image area
        _image = new TextureRect
        {
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            TextureScale = new Vector2(DefaultImageScale, DefaultImageScale),
            VerticalExpand = false, // Don't expand vertically
            Margin = new Thickness(10, 5, 10, 5)
        };

        // Text container for multiple lines
        _textContainer = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            VerticalExpand = true,
            HorizontalExpand = true,
            SeparationOverride = LineSpacing
        };

        container.AddChild(_image);
        container.AddChild(_textContainer);
        AddChild(container);
    }

    private string GetDescriptionText()
    {
        var text = "";
        foreach (var line in _textLines)
        {
            text += line.Text + "\n";
        }
        return text.TrimEnd();
    }

    private void SetDescriptionText(string text)
    {
        if (_textContainer == null) return;

        // Clear existing labels
        foreach (var line in _textLines)
        {
            _textContainer.RemoveChild(line);
        }
        _textLines.Clear();

        // Split text into lines
        var lines = text.Split("\\n");

        // Create a new label for each line
        foreach (var line in lines)
        {
            var label = new Label
            {
                Text = line.Trim(),
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Top,
                HorizontalExpand = true,
                ClipText = false
            };

            _textLines.Add(label);
            _textContainer.AddChild(label);
        }
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();

        if (_styleBox == null) return;

        // Update colors based on interaction state
        if (DrawMode == DrawModeEnum.Hover && HoverColor != Color.Transparent)
        {
            _styleBox.BackgroundColor = HoverColor;
        }
        else if (DrawMode == DrawModeEnum.Pressed && PressedColor != Color.Transparent)
        {
            _styleBox.BackgroundColor = PressedColor;
        }
        else
        {
            _styleBox.BackgroundColor = BackgroundColor;
        }
    }
}
