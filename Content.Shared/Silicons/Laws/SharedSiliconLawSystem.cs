// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Arendian <137322659+Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Jajsha <101492056+Zap527@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Emag.Systems;
using Content.Shared.Popups;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Wires;

namespace Content.Shared.Silicons.Laws;

/// <summary>
/// This handles getting and displaying the laws for silicons.
/// </summary>
public abstract partial class SharedSiliconLawSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        InitializeUpdater();
        SubscribeLocalEvent<EmagSiliconLawComponent, GotEmaggedEvent>(OnGotEmagged);
        SubscribeLocalEvent<EmagSiliconLawComponent, OnAttemptEmagEvent>(OnAttemptEmag);
    }

    protected virtual void OnAttemptEmag(EntityUid uid, EmagSiliconLawComponent component, ref OnAttemptEmagEvent args)
    {
        //prevent self emagging
        if (uid == args.UserUid)
        {
            _popup.PopupClient(Loc.GetString("law-emag-cannot-emag-self"), uid, args.UserUid);
            args.Handled = true;
            return;
        }

        if (component.RequireOpenPanel &&
            TryComp<WiresPanelComponent>(uid, out var panel) &&
            !panel.Open)
        {
            _popup.PopupClient(Loc.GetString("law-emag-require-panel"), uid, args.UserUid);
            args.Handled = true;
        }

    }

    protected virtual void OnGotEmagged(EntityUid uid, EmagSiliconLawComponent component, ref GotEmaggedEvent args)
    {
        component.OwnerName = Name(args.UserUid);
        args.Handled = true;
    }
}
