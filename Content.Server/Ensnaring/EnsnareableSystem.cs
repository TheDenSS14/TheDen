// SPDX-FileCopyrightText: 2022 Nemanja
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Vordenburg
// SPDX-FileCopyrightText: 2023 keronshb
// SPDX-FileCopyrightText: 2024 Ghost581
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.Ensnaring;
using Content.Shared.Ensnaring.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Popups;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Ensnaring;

public sealed partial class EnsnareableSystem : SharedEnsnareableSystem
{
    [Dependency] private readonly ContainerSystem _container = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        InitializeEnsnaring();

        SubscribeLocalEvent<EnsnareableComponent, ComponentInit>(OnEnsnareableInit);
        SubscribeLocalEvent<EnsnareableComponent, EnsnareableDoAfterEvent>(OnDoAfter);
    }

    private void OnEnsnareableInit(EntityUid uid, EnsnareableComponent component, ComponentInit args)
    {
        component.Container = _container.EnsureContainer<Container>(uid, "ensnare");
    }

    private void OnDoAfter(EntityUid uid, EnsnareableComponent component, DoAfterEvent args)
    {
        if (args.Args.Target == null)
            return;

        if (args.Handled || !TryComp<EnsnaringComponent>(args.Args.Used, out var ensnaring))
            return;

        if (args.Cancelled || !_container.Remove(args.Args.Used.Value, component.Container))
        {
            _popup.PopupEntity(Loc.GetString("ensnare-component-try-free-fail", ("ensnare", args.Args.Used)), uid, uid, PopupType.MediumCaution);
            return;
        }

        component.IsEnsnared = component.Container.ContainedEntities.Count > 0;
        Dirty(uid, component);
        ensnaring.Ensnared = null;

        if (ensnaring.DestroyOnRemove)
            QueueDel(args.Args.Used);
        else
            _hands.PickupOrDrop(args.Args.User, args.Args.Used.Value);

        _popup.PopupEntity(Loc.GetString("ensnare-component-try-free-complete", ("ensnare", args.Args.Used)), uid, uid, PopupType.Medium);

        UpdateAlert(args.Args.Target.Value, component);
        var ev = new EnsnareRemoveEvent(ensnaring.WalkSpeed, ensnaring.SprintSpeed);
        RaiseLocalEvent(uid, ev);

        args.Handled = true;
    }
}
