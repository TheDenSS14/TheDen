// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: MIT

using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._DEN.Holosign.UI;

[UsedImplicitly]
public sealed class LabelableHolosignProjectorDescriptionBUI : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    
    [ViewVariables]
    private LabelableHolosignProjectorWindow? _window;
    
    public LabelableHolosignProjectorDescriptionBUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
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
    
    private void OnDescriptionChanged(string description, bool isNsfw)
    {
        if (_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector) &&
            projector.BarrierDescription.Equals(description) && projector.IsNsfw == isNsfw)
            return;

        SendPredictedMessage(new LabelableHolosignDescriptionMessage(description, isNsfw));
    }

    public void Reload()
    {
        if (_window == null || !_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector))
            return;

        _window.SetCurrentDescription(projector.BarrierDescription);
        _window.SetCurrentNsfw(projector.IsNsfw);
    }
}
