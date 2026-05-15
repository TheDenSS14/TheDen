// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.UserInterface.Controls;
using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client._DEN.Holosign.UI;

[UsedImplicitly]
public sealed class LabelableHolosignProjectorSignBUI : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    [ViewVariables] private SimpleRadialMenu? _menu;
    
    public LabelableHolosignProjectorSignBUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();
        
        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);

        var controls = new List<RadialMenuActionOptionBase>();
        controls.Add(new RadialMenuActionOption<LabelableHolosignUIKey>(OpenDescriptionWindow, LabelableHolosignUIKey.Description)
        {
            IconSpecifier = RadialMenuIconSpecifier.With(new SpriteSpecifier.Texture(new("/Textures/Interface/examine-star.png"))),
            ToolTip = Loc.GetString("labelable-holoprojector-ui-set-description")
        });
        
        if (_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector))
        {
            foreach (var proto in projector.SignProtos)
            {
                if (_protoManager.TryIndex(proto, out var entProto))
                {
                    controls.Add(
                        new RadialMenuActionOption<EntProtoId>(SelectSignProto, proto)
                        {
                            IconSpecifier = RadialMenuIconSpecifier.With(entProto),
                            ToolTip = Loc.GetString(entProto.Name),
                        });
                }
            }
        }
        _menu.SetButtons(controls);
    }

    private void SelectSignProto(EntProtoId protoId)
    {
        if (!_entManager.TryGetComponent(Owner, out LabelableHolosignProjectorComponent? projector))
            return;
        
        var selected = projector.SignProtos.IndexOf(protoId);
        SendPredictedMessage(new LabelableHolosignSignChosen(selected));
    }

    private void OpenDescriptionWindow(LabelableHolosignUIKey key)
    {
        SendPredictedMessage(new LabelableHolosignOpenOtherUI());
    }
}
