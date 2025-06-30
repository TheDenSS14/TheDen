// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Advertise.Components;
using Content.Server.Chat.Systems;
using Content.Server.UserInterface;
using Content.Shared.Advertise;
using Content.Shared.Chat;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using ActivatableUIComponent = Content.Shared.UserInterface.ActivatableUIComponent;

namespace Content.Server.Advertise;

public sealed partial class SpeakOnUIClosedSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ChatSystem _chat = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeakOnUIClosedComponent, BoundUIClosedEvent>(OnBoundUIClosed);
    }
    private void OnBoundUIClosed(Entity<SpeakOnUIClosedComponent> entity, ref BoundUIClosedEvent args)
    {
        if (!TryComp(entity, out ActivatableUIComponent? activatable) || !args.UiKey.Equals(activatable.Key))
            return;

        if (entity.Comp.RequireFlag && !entity.Comp.Flag)
            return;

        TrySpeak((entity, entity.Comp));
    }

    public bool TrySpeak(Entity<SpeakOnUIClosedComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp))
            return false;

        if (!entity.Comp.Enabled)
            return false;

        if (!_prototypeManager.TryIndex(entity.Comp.Pack, out MessagePackPrototype? messagePack))
            return false;

        var message = Loc.GetString(_random.Pick(messagePack.Messages), ("name", Name(entity)));
        _chat.TrySendInGameICMessage(entity, message, InGameICChatType.Speak, true);
        entity.Comp.Flag = false;
        return true;
    }

    public bool TrySetFlag(Entity<SpeakOnUIClosedComponent?> entity, bool value = true)
    {
        if (!Resolve(entity, ref entity.Comp))
            return false;

        entity.Comp.Flag = value;
        return true;
    }
}
