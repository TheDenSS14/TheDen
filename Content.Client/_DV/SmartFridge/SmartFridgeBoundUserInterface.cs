// SPDX-FileCopyrightText: 2025 Perry Fraser
// SPDX-FileCopyrightText: 2025 pathetic meowmeow
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Client.UserInterface;
using Robust.Shared.Input;
using Content.Client.UserInterface.Controls;
using Content.Shared._DV.SmartFridge;

namespace Content.Client._DV.SmartFridge;

public sealed class SmartFridgeBoundUserInterface : BoundUserInterface
{
    private SmartFridgeMenu? _menu;

    public SmartFridgeBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<SmartFridgeMenu>();
        _menu.OnItemSelected += OnItemSelected;
        _menu.OnRemoveButtonPressed += OnRemoveButtonPressed;
        Refresh();
    }

    public void Refresh()
    {
        if (_menu is not {} menu || !EntMan.TryGetComponent(Owner, out SmartFridgeComponent? fridge))
            return;

        menu.Populate((Owner, fridge));
    }

    private void OnItemSelected(GUIBoundKeyEventArgs args, ListData data)
    {
        if (args.Function != EngineKeyFunctions.UIClick)
            return;

        if (data is not SmartFridgeListData entry)
            return;
        SendPredictedMessage(new SmartFridgeDispenseItemMessage(entry.Entry));
    }

    private void OnRemoveButtonPressed(SmartFridgeListData data)
    {
        SendPredictedMessage(new SmartFridgeRemoveEntryMessage(data.Entry));
    }
}
