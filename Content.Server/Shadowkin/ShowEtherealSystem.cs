// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 RedFoxIV <38788538+RedFoxIV@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Shadowkin;
using Content.Shared.Eye;
using Robust.Server.GameObjects;
using Content.Shared.Inventory.Events;
using Content.Shared.Interaction.Events;
using Robust.Shared.Timing;
using Content.Shared.Popups;
using Content.Shared.Clothing.Components;

namespace Content.Server.Shadowkin;
public sealed class ShowEtherealSystem : EntitySystem
{
    [Dependency] private readonly EyeSystem _eye = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ShowEtherealComponent, MapInitEvent>(OnInit);
        SubscribeLocalEvent<ShowEtherealComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ShowEtherealComponent, GotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<ShowEtherealComponent, GotUnequippedEvent>(OnUnequipped);
        SubscribeLocalEvent<ShowEtherealComponent, InteractionAttemptEvent>(OnInteractionAttempt);
        SubscribeLocalEvent<ShowEtherealComponent, AttackAttemptEvent>(OnAttackAttempt);
    }

    private void OnInit(EntityUid uid, ShowEtherealComponent component, MapInitEvent args)
    {
        Toggle(uid, true);
    }

    public void OnShutdown(EntityUid uid, ShowEtherealComponent component, ComponentShutdown args)
    {
        Toggle(uid, false);
    }

    private void OnEquipped(EntityUid uid, ShowEtherealComponent component, GotEquippedEvent args)
    {
        if (!TryComp<ClothingComponent>(uid, out var clothing)
            || !clothing.Slots.HasFlag(args.SlotFlags))
            return;

        EnsureComp<ShowEtherealComponent>(args.Equipee);
    }

    private void OnUnequipped(EntityUid uid, ShowEtherealComponent component, GotUnequippedEvent args)
    {
        RemComp<ShowEtherealComponent>(args.Equipee);
    }

    private void Toggle(EntityUid uid, bool toggle)
    {
        if (!TryComp<EyeComponent>(uid, out var eye))
            return;

        if (toggle)
        {
            _eye.SetVisibilityMask(uid, eye.VisibilityMask | (int) (VisibilityFlags.Ethereal), eye);
            return;
        }
        else if (HasComp<EtherealComponent>(uid))
            return;

        _eye.SetVisibilityMask(uid, (int) VisibilityFlags.Normal, eye);
    }

    private void OnInteractionAttempt(EntityUid uid, ShowEtherealComponent component, InteractionAttemptEvent args)
    {
        if (HasComp<EtherealComponent>(uid)
            || !HasComp<EtherealComponent>(args.Target))
            return;

        args.Cancelled = true;
        if (_gameTiming.InPrediction)
            return;

        _popup.PopupEntity(Loc.GetString("ethereal-pickup-fail"), args.Target.Value, uid);
    }

    private void OnAttackAttempt(EntityUid uid, ShowEtherealComponent component, AttackAttemptEvent args)
    {
        if (HasComp<EtherealComponent>(uid)
            || !HasComp<EtherealComponent>(args.Target))
            return;

        args.Cancel();
    }
}
