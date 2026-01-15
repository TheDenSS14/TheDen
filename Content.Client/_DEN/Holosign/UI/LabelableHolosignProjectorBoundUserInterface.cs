using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using Robust.Client.UserInterface;


namespace Content.Client._DEN.Holosign.UI;


public sealed class LabelableHolosignProjectorBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [ViewVariables]
    private LabelableHolosignProjectorWindow? _window;

    public LabelableHolosignProjectorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<LabelableHolosignProjectorWindow>();

        _window.OnDescriptionChanged += OnDescriptionChanged;
        Reload();
    }

    private void OnDescriptionChanged(string description)
    {
        if (_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector) &&
            projector.BarrierDescription.Equals(description))
            return;

        SendPredictedMessage(new LabelableHolosignChangedMessage(description));
    }

    public void Reload()
    {
        if (_window == null || !_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector))
            return;

        _window.SetCurrentDescription(projector.BarrierDescription);
    }
}
