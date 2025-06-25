// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Popups;
using Content.Server.Power.Components;
using Content.Shared.Power;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;
using Content.Shared.UserInterface;
using JetBrains.Annotations;
using Content.Shared.Wires;
using Content.Server.UserInterface;
using Content.Shared.Power.Components;
using ActivatableUISystem = Content.Shared.UserInterface.ActivatableUISystem;

namespace Content.Server.Power.EntitySystems;

public sealed class ActivatableUIRequiresPowerSystem : EntitySystem
{
    [Dependency] private readonly ActivatableUISystem _activatableUI = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ActivatableUIRequiresPowerComponent, ActivatableUIOpenAttemptEvent>(OnActivate);
        SubscribeLocalEvent<ActivatableUIRequiresPowerComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnActivate(EntityUid uid, ActivatableUIRequiresPowerComponent component, ActivatableUIOpenAttemptEvent args)
    {
        if (args.Cancelled) return;
        if (this.IsPowered(uid, EntityManager)) return;
        if (TryComp<WiresPanelComponent>(uid, out var panel) && panel.Open)
            return;
        _popup.PopupCursor(Loc.GetString("base-computer-ui-component-not-powered", ("machine", uid)), args.User);
        args.Cancel();
    }

    private void OnPowerChanged(EntityUid uid, ActivatableUIRequiresPowerComponent component, ref PowerChangedEvent args)
    {
        if (!args.Powered)
            _activatableUI.CloseAll(uid);
    }
}
