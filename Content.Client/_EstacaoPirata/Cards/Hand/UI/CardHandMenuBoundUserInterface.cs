// SPDX-FileCopyrightText: 2025 RadsammyT
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.UserInterface.Controls;
using Content.Shared._DEN.Unrotting;
using Content.Shared._EstacaoPirata.Cards.Card;
using Content.Shared._EstacaoPirata.Cards.Hand;
using Content.Shared._EstacaoPirata.Cards.Stack;
using Content.Shared.RCD;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._EstacaoPirata.Cards.Hand.UI;

[UsedImplicitly]
public sealed class CardHandMenuBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IPlayerManager _playerMan = default!;

    private SimpleRadialMenu? _menu;

    public CardHandMenuBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<CardStackComponent>(Owner, out var stack))
            return;

        var actions = GetCardStackActions(stack);

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);
        _menu.SetButtons(actions);
        _menu.OpenOverMouseScreenPosition();
    }

    private IEnumerable<RadialMenuActionOption<NetEntity>> GetCardStackActions(CardStackComponent stack)
    {
        List<RadialMenuActionOption<NetEntity>> actions = new();

        foreach (var card in stack.Cards)
        {
            if (_playerMan.LocalSession == null
                || !EntMan.TryGetComponent<CardComponent>(card, out var cardComp))
                continue;

            var networkedCard = EntMan.GetNetEntity(card);
            string cardName;

            if (cardComp.Flipped && EntMan.TryGetComponent<MetaDataComponent>(card, out var metadata))
                cardName = metadata.EntityName;
            else
                cardName = Loc.GetString(cardComp.Name);

            if (!EntMan.TryGetComponent<SpriteComponent>(card, out var sprite) || sprite.Icon == null)
                continue;

            var iconSpecifier = RadialMenuIconSpecifier.With(card);
            var action = new RadialMenuActionOption<NetEntity>(SendCardHandDrawMessage, networkedCard)
            {
                IconSpecifier = iconSpecifier,
                ToolTip = cardName
            };

            actions.Add(action);
        }

        return actions;
    }

    public void SendCardHandDrawMessage(NetEntity e) => SendMessage(new CardHandDrawMessage(e));
}
