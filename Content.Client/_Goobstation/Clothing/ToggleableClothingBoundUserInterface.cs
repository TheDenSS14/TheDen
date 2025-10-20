// SPDX-FileCopyrightText: 2025 Eris
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.UserInterface.Controls;
using Content.Shared.Clothing.Components;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;


namespace Content.Client._Goobstation.Clothing;

public sealed class ToggleableClothingBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;

    private SimpleRadialMenu? _menu;

    public ToggleableClothingBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<ToggleableClothingComponent>(Owner, out var clothing)
            || clothing.Container is not { } clothingContainer)
            return;

        var actions = GetToggleableClothingActions(clothing, clothingContainer);

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);
        _menu.SetButtons(actions);
        _menu.OpenOverMouseScreenPosition();
    }

    private IEnumerable<RadialMenuActionOption<NetEntity>> GetToggleableClothingActions(
        ToggleableClothingComponent clothing,
        Container clothingContainer)
    {
        var actions = new List<RadialMenuActionOption<NetEntity>>();

        foreach (var pair in clothing.ClothingUids)
        {
            if (!EntMan.TryGetComponent(pair.Key, out MetaDataComponent? metaData) || metaData.EntityPrototype == null)
                continue;

            // Change tooltip text if attached clothing is toggle/untoggled
            var attached = clothingContainer.Contains(pair.Key) ? clothing.AttachTooltip : clothing.UnattachTooltip;
            var tooltipText = Loc.GetString(attached);
            var netEntity = EntMan.GetNetEntity(pair.Key);
            var action = new RadialMenuActionOption<NetEntity>(SendToggleableClothingMessage, netEntity)
            {
                ToolTip = tooltipText,
                IconSpecifier = RadialMenuIconSpecifier.With(metaData.EntityPrototype)
            };

            actions.Add(action);
        }

        return actions;
    }

    private void SendToggleableClothingMessage(NetEntity uid)
    {
        var message = new ToggleableClothingUiMessage(uid);
        SendPredictedMessage(message);
    }
}
