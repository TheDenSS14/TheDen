// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jacob Tong <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2022 KIBORG04 <bossmira4@gmail.com>
// SPDX-FileCopyrightText: 2022 Kara D <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2023 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Ygg01 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 M3739 <47579354+M3739@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tabitha <64847293+KyuPolaris@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Systems;
using Content.Server.Kitchen.Components;
using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Body.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Verbs;
using Content.Shared.Destructible;
using Content.Shared.DoAfter;
using Content.Shared.Kitchen;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using Content.Server.Atmos.Rotting;
using Content.Shared._DEN.Kitchen;

namespace Content.Server.Kitchen.EntitySystems;

public sealed class SharpSystem : EntitySystem
{
    [Dependency] private readonly BodySystem _bodySystem = default!;
    [Dependency] private readonly SharedButcherySystem _butcherySystem = default!;
    [Dependency] private readonly SharedDestructibleSystem _destructibleSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly ContainerSystem _containerSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SharpComponent, AfterInteractEvent>(OnAfterInteract, before: [typeof(UtensilSystem)]);
        SubscribeLocalEvent<SharpComponent, SharpDoAfterEvent>(OnDoAfter);

        SubscribeLocalEvent<ButcherableComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);
    }

    private void OnAfterInteract(EntityUid uid, SharpComponent component, AfterInteractEvent args)
    {
        if (args.Handled || args.Target is null || !args.CanReach)
            return;

        if (TryStartButcherDoafter(uid, args.Target.Value, args.User))
            args.Handled = true;
    }

    private bool TryStartButcherDoafter(EntityUid knife, EntityUid target, EntityUid user)
    {
        if (!TryComp<ButcherableComponent>(target, out var butcher))
            return false;

        if (!TryComp<SharpComponent>(knife, out var sharp))
            return false;

        if (TryComp<MobStateComponent>(target, out var mobState) && !_mobStateSystem.IsDead(target, mobState))
            return false;

        if (butcher.Type != ButcheringType.Knife && target != user)
        {
            _popupSystem.PopupEntity(Loc.GetString("butcherable-different-tool", ("target", target)), knife, user);
            return false;
        }

        if (!sharp.Butchering.Add(target))
            return false;

        var doAfter =
            new DoAfterArgs(EntityManager, user, sharp.ButcherDelayModifier * butcher.ButcherDelay, new SharpDoAfterEvent(), knife, target: target, used: knife)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                NeedHand = true
            };
        _doAfterSystem.TryStartDoAfter(doAfter);
        return true;
    }

    private void OnDoAfter(EntityUid uid, SharpComponent component, DoAfterEvent args)
    {
        if (args.Handled || !TryComp<ButcherableComponent>(args.Args.Target, out var butcher))
            return;

        if (args.Cancelled)
        {
            component.Butchering.Remove(args.Args.Target.Value);
            return;
        }

        component.Butchering.Remove(args.Args.Target.Value);

        if (_containerSystem.IsEntityInContainer(args.Args.Target.Value))
        {
            args.Handled = true;
            return;
        }

        var popupEnt = _butcherySystem.SpawnButcherableProducts(args.Args.Target.Value, butcher);
        var hasBody = TryComp<BodyComponent>(args.Args.Target.Value, out var body);

        // only show a big popup when butchering living things.
        var popupType = PopupType.Small;
        if (hasBody)
            popupType = PopupType.LargeCaution;

        if (popupEnt != null)
            _popupSystem.PopupEntity(Loc.GetString("butcherable-knife-butchered-success",
                ("target", args.Args.Target.Value),
                ("knife", uid)),
                popupEnt.Value, args.Args.User, popupType);

        if (hasBody && butcher.GibBody) // Floof - added additional check
            _bodySystem.GibBody(args.Args.Target.Value, body: body, acidify: butcher.GibOrgans, contents: butcher.GibContents);

        _destructibleSystem.DestroyEntity(args.Args.Target.Value);

        args.Handled = true;

        _adminLogger.Add(LogType.Gib,
            $"{EntityManager.ToPrettyString(args.User):user} " +
            $"has butchered {EntityManager.ToPrettyString(args.Target):target} " +
            $"with {EntityManager.ToPrettyString(args.Used):knife}");
    }

    private void OnGetInteractionVerbs(EntityUid uid, ButcherableComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        if (component.Type != ButcheringType.Knife || args.Hands == null || !args.CanAccess || !args.CanInteract)
            return;

        bool disabled = false;
        string? message = null;

        if (!HasComp<SharpComponent>(args.Using))
        {
            disabled = true;
            message = Loc.GetString("butcherable-need-knife",
                ("target", uid));
        }
        else if (_containerSystem.IsEntityInContainer(uid))
        {
            message = Loc.GetString("butcherable-not-in-container",
                ("target", uid));
            disabled = true;
        }
        else if (TryComp<MobStateComponent>(uid, out var state) && !_mobStateSystem.IsDead(uid, state))
        {
            disabled = true;
            message = Loc.GetString("butcherable-mob-isnt-dead");
        }

        InteractionVerb verb = new()
        {
            Act = () =>
            {
                if (!disabled)
                    TryStartButcherDoafter(args.Using!.Value, args.Target, args.User);
            },
            Message = message,
            Disabled = disabled,
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/cutlery.svg.192dpi.png")),
            Text = Loc.GetString("butcherable-verb-name"),
        };

        args.Verbs.Add(verb);
    }
}
