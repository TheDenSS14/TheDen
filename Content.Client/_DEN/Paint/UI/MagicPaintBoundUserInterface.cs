using Content.Shared._DEN.Paint;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._DEN.Paint.UI;

public sealed class MagicPaintBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    [ViewVariables]
    private MagicPaintWindow? _menu;

    public MagicPaintBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey) { }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<MagicPaintWindow>();
        _menu.OnColorChanged += SelectColor;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is MagicPaintInterfaceState magicPaintState)
            _menu?.SetColor(magicPaintState.Color);
    }

    private void SelectColor(Color color) => SendMessage(new MagicPaintColorMessage(color));
}
