// SPDX-FileCopyrightText: 2025 Aviu00
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.UserInterface.Controls;
using Content.Shared._Goobstation.Weapons.AmmoSelector;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._Goobstation.AmmoSelector;

[UsedImplicitly]
public sealed class AmmoSelectorMenuBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;

    private SimpleRadialMenu? _menu;

    public AmmoSelectorMenuBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<AmmoSelectorComponent>(Owner, out var ammoSelector))
            return;

        var actions = GetAmmoSelectorActions(ammoSelector.Prototypes);

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);
        _menu.SetButtons(actions);
        _menu.OpenOverMouseScreenPosition();
    }

    private IEnumerable<RadialMenuActionOption<ProtoId<SelectableAmmoPrototype>>> GetAmmoSelectorActions(HashSet<ProtoId<SelectableAmmoPrototype>> protoIds)
    {
        var actions = new List<RadialMenuActionOption<ProtoId<SelectableAmmoPrototype>>>();

        foreach (var selectableAmmoId in protoIds)
        {
            if (!_protoMan.TryIndex(selectableAmmoId, out var selectableAmmo))
                continue;

            var action = new RadialMenuActionOption<ProtoId<SelectableAmmoPrototype>>(OnAmmoSelected, selectableAmmoId)
            {
                ToolTip = selectableAmmo.Desc,
                IconSpecifier = RadialMenuIconSpecifier.With(selectableAmmo.Icon)
            };

            actions.Add(action);
        }

        return actions;
    }

    private void OnAmmoSelected(ProtoId<SelectableAmmoPrototype> protoId) => SendPredictedMessage(new AmmoSelectedMessage(protoId));
}
