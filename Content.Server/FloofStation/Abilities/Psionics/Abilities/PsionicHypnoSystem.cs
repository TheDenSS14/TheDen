// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2025 Rosycup
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server._Floof.Consent;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Actions.Events;
using Content.Shared.Floofstation.Hypno;
using Content.Shared.Popups;
using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Robust.Shared.Utility;
using Content.Shared.Verbs;
using Content.Shared.Examine;
using Content.Shared.Mood;
using Content.Server.Chat.Managers;
using Content.Shared.Chat;
using Robust.Server.Player;
using Content.Shared.Database;
using Content.Shared.Administration.Logs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Mobs.Components;
using Content.Shared.Psionics;
using Content.Shared.Mind.Components;


namespace Content.Server.Abilities.Psionics;


public sealed class PsionicHypnoSystem : EntitySystem
{
    [Dependency] private readonly SharedPsionicAbilitiesSystem _psionics = default!;
    [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popups = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly ConsentSystem _consent = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PsionicHypnoComponent, HypnoPowerActionEvent>(OnPowerUsed);
        SubscribeLocalEvent<PsionicHypnoComponent, GetVerbsEvent<InnateVerb>>(ReleaseSubjectVerb);
        SubscribeLocalEvent<HypnotizedComponent, DispelledEvent>(OnDispelledHypnotized);
        SubscribeLocalEvent<PsionicHypnoComponent, DispelledEvent>(OnDispelled);
        SubscribeLocalEvent<PsionicHypnoComponent, PsionicHypnosisDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<HypnotizedComponent, GetVerbsEvent<InnateVerb>>(BreakHypnoVerb);
        SubscribeLocalEvent<HypnotizedComponent, OnMindbreakEvent>(OnMindbreak);
        SubscribeLocalEvent<HypnotizedComponent, ExaminedEvent>((uid, _, args) => OnExamine(uid, args));
    }

    public void Hypnotize(EntityUid uid, EntityUid target, PsionicHypnoComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        EnsureComp<HypnotizedComponent>(target, out var hypnotized);
        hypnotized.Masters.Add(uid);

        Dirty(target, hypnotized);
        _adminLog.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(uid)} hypnotized {ToPrettyString(target)}");
        RaiseLocalEvent(target, new MoodEffectEvent("BeingHypnotized"));

        if (_playerManager.TryGetSessionByEntity(target, out var session)
            || session is not null)
        {
            var message = Loc.GetString("hypnotized", ("entity", uid));
            _chatManager.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                session.Channel);
        }
    }

    public bool IsHypnotized(EntityUid uid, EntityUid? by = null)
    {
        if (!TryComp<HypnotizedComponent>(uid, out var hypnotized))
            return false;

        return !by.HasValue || hypnotized.Masters.Contains(by.Value);
    }

    public void StopAllHypno(Entity<HypnotizedComponent> hypnotized, EntityUid? brokenBy = null)
    {
        _adminLog.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(hypnotized)} is no longer hypnotized.");
        _popups.PopupEntity(Loc.GetString("hypno-free"), hypnotized, hypnotized, PopupType.LargeCaution);

        RaiseLocalEvent(hypnotized, new MoodEffectEvent("LostHypnosis"));
        NotifyMasters(hypnotized);
        RemComp<HypnotizedComponent>(hypnotized);
    }

    public void StopHypno(Entity<HypnotizedComponent> hypnotized, EntityUid master)
    {
        if (!hypnotized.Comp.Masters.Remove(master))
            return;

        _adminLog.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(hypnotized)} is no longer hypnotized by {ToPrettyString(master)}.");
        _popups.PopupEntity(Loc.GetString("hypno-free-specific", ("master", master)), hypnotized, hypnotized, PopupType.LargeCaution);
        Dirty(hypnotized, hypnotized.Comp);
    }

    private void NotifyMasters(Entity<HypnotizedComponent> hypnotized)
    {
        foreach (var master in hypnotized.Comp.Masters)
        {
            _popups.PopupEntity(Loc.GetString("lost-subject"), master, master, PopupType.LargeCaution);
            RaiseLocalEvent(master, new HypnosisBrokenEvent(hypnotized));
        }
    }

    private void OnPowerUsed(EntityUid uid, PsionicHypnoComponent component, HypnoPowerActionEvent args)
    {
        if (!_psionics.OnAttemptPowerUse(args.Performer, "hypno")
            || !TryComp<MobStateComponent>(args.Target, out var mob)
            || _mobState.IsDead(args.Target, mob)
            || _mobState.IsCritical(args.Target, mob))
            return;

        if (!_consent.HasConsent(args.Target, "Hypno"))
        {
            _popups.PopupEntity(Loc.GetString("has-no-consent", ("target", args.Target)), uid, uid, PopupType.Large);
            return;
        }

        if (TryComp<HypnotizedComponent>(uid, out var hypnotized) && hypnotized.Masters.Contains(uid))
        {
            _popups.PopupEntity(Loc.GetString("hypno-already-under", ("target", uid)), uid, uid, PopupType.Large);
            return;
        }

        _doAfterSystem.TryStartDoAfter(new(EntityManager, uid, component.UseDelay, new PsionicHypnosisDoAfterEvent(1), uid, target: args.Target)
        {
            Hidden = true,
            BreakOnMove = true,
            BreakOnDamage = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;

        _popups.PopupEntity(Loc.GetString("hypno-start", ("target", args.Target)), uid, uid, PopupType.LargeCaution);
        _popups.PopupEntity(Loc.GetString("hypno-phase-1", ("target", uid)), args.Target, args.Target, PopupType.Small);

        args.Handled = true;
        _psionics.LogPowerUsed(args.Performer, "hypno");
    }

    private void OnDispelled(EntityUid uid, PsionicHypnoComponent component, DispelledEvent args)
    {
        if (component.DoAfter is null)
            return;

        _doAfterSystem.Cancel(component.DoAfter);
        component.DoAfter = null;
        args.Handled = true;
    }

    private void OnDispelledHypnotized(EntityUid uid, HypnotizedComponent component, DispelledEvent args)
    {
        StopAllHypno((uid, component));
    }

    private void OnMindbreak(EntityUid uid, HypnotizedComponent component, ref OnMindbreakEvent args)
    {
        StopAllHypno((uid, component));
    }

    private void ReleaseSubjectVerb(EntityUid uid, PsionicHypnoComponent component, GetVerbsEvent<InnateVerb> args)
    {
        if (args.User == args.Target
            || !TryComp<HypnotizedComponent>(args.Target, out var hypno)
            || args.User != uid)
            return;

        InnateVerb verbReleaseHypno = new()
        {
            Act = () => StopHypno((args.Target, hypno), uid),
            Text = Loc.GetString("hypno-release"),
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/_Floof/Interface/Actions/hypno.png")),
            Priority = 1
        };

        args.Verbs.Add(verbReleaseHypno);
    }

    private void BreakHypnoVerb(EntityUid uid, HypnotizedComponent component, GetVerbsEvent<InnateVerb> args)
    {
        if (args.User != args.Target)
            return;

        InnateVerb verbBreakHypno = new()
        {
            Act = () => StopAllHypno((uid, component)),
            Text = Loc.GetString("hypno-break"),
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/_Floof/Interface/Actions/hypno.png")),
            Priority = 1
        };
        args.Verbs.Add(verbBreakHypno);
    }

    private void OnDoAfter(EntityUid uid, PsionicHypnoComponent component, PsionicHypnosisDoAfterEvent args)
    {
        component.DoAfter = null;

        if (args.Target is null
            || args.Cancelled)
            return;

        DoPhasePopup(uid, component, args);
    }

    private void DoPhasePopup(EntityUid uid, PsionicHypnoComponent component, PsionicHypnosisDoAfterEvent args)
    {
        if (args.Target is null)
            return;

        if (args.Phase != 1 && args.Phase != 2)
        {
            _popups.PopupEntity(Loc.GetString("hypno-success", ("initiator", uid), ("target", args.Target)),
                uid,
                uid,
                PopupType.LargeCaution);
            Hypnotize(uid, args.Target.Value, component);
            return;
        }

        var phaseValue = args.Phase + 1;
        var phaseMessageId = $"hypno-phase-{phaseValue}";

        _popups.PopupEntity(Loc.GetString(phaseMessageId, ("initiator", uid), ("target", args.Target)),
            args.Target.Value,
            args.Target.Value,
            PopupType.Medium);

        _doAfterSystem.TryStartDoAfter(new(
            EntityManager,
            uid,
            component.UseDelay,
            new PsionicHypnosisDoAfterEvent(phaseValue),
            uid,
            target: args.Target)
        {
            Hidden = true,
            BreakOnMove = true,
            BreakOnDamage = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;
    }

    private void OnExamine(EntityUid uid, ExaminedEvent args)
    {
        if (args.IsInDetailsRange)
            args.PushMarkup(Loc.GetString("examined-hypno"), -1);
    }
}

public record struct HypnosisBrokenEvent(Entity<HypnotizedComponent> hypnotized)
{
    public Entity<HypnotizedComponent> Hypnotized { get; set; } = hypnotized;
}
