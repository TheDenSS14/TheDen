// SPDX-FileCopyrightText: 2023 Debug <sidneymaatman@gmail.com>
// SPDX-FileCopyrightText: 2023 JJ <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Consent;
using Content.Shared.Actions;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Speech;
using Content.Shared.Stealth.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs;
using Content.Shared.Damage;
using Content.Server.Mind;
using Content.Shared.Mobs.Systems;
using Content.Server.Popups;
using Content.Server.Psionics;
using Content.Shared.Mind;
using Content.Shared.Actions.Events;
using Content.Server.DoAfter;
using Content.Server.Ghost;
using Content.Shared.Consent;
using Content.Shared.DoAfter;
using Robust.Shared.Prototypes;

namespace Content.Server.Abilities.Psionics
{
    public sealed class MindSwapPowerSystem : EntitySystem
    {
        [Dependency] private readonly SharedActionsSystem _actions = default!;
        [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
        [Dependency] private readonly SharedPsionicAbilitiesSystem _psionics = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly MindSystem _mindSystem = default!;
        [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly ConsentSystem _consentSystem = default!;

        private ProtoId<ConsentTogglePrototype> _mindSwapConsent = "MindSwap";

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<MindSwapPowerComponent, MindSwapPowerActionEvent>(OnPowerUsed);
            SubscribeLocalEvent<PsionicComponent, MindSwapPowerDoAfterEvent>(OnDoAfter);
            SubscribeLocalEvent<MindSwappedComponent, MindSwapPowerReturnActionEvent>(OnPowerReturned);
            SubscribeLocalEvent<MindSwappedComponent, DispelledEvent>(OnDispelled);
            SubscribeLocalEvent<MindSwappedComponent, MobStateChangedEvent>(OnMobStateChanged);
            SubscribeLocalEvent<GhostAttemptHandleEvent>(OnGhostAttempt);

            SubscribeLocalEvent<MindSwappedComponent, ComponentInit>(OnSwapInit);
        }

        private void OnPowerUsed(EntityUid uid, MindSwapPowerComponent component, MindSwapPowerActionEvent args)
        {
            if (!_psionics.OnAttemptPowerUse(args.Performer, "mind swap")
                || !(TryComp<DamageableComponent>(args.Target, out var damageable) && damageable.DamageContainerID == "Biological"))
                return;

            if (!_consentSystem.HasConsent(args.Target, _mindSwapConsent))
            {
                _popupSystem.PopupEntity(
                    Loc.GetString("consent-MindSwap-no-consent",
                        ("target", args.Target)),
                    args.Performer);
                return;
            }

            _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, args.Performer, component.UseDelay, new MindSwapPowerDoAfterEvent(), args.Performer, target: args.Target)
            {
                Hidden = true,
                BreakOnMove = true,
                BreakOnDamage = true,
            }, out var doAfterId);

            if (TryComp<PsionicComponent>(uid, out var magic))
                magic.DoAfter = doAfterId;

            _psionics.LogPowerUsed(args.Performer, "mind swap");
            args.Handled = true;
        }

        private void OnDoAfter(EntityUid uid, PsionicComponent component, MindSwapPowerDoAfterEvent args)
        {
            if (component is null)
                return;
            component.DoAfter = null;

            if (args.Target is null
                || args.Cancelled)
                return;

            Swap(uid, args.Target.Value);
        }

        private void OnPowerReturned(EntityUid uid, MindSwappedComponent component, MindSwapPowerReturnActionEvent args)
        {
            if (HasComp<PsionicInsulationComponent>(component.OriginalEntity) || HasComp<PsionicInsulationComponent>(uid))
                return;

            if (HasComp<MobStateComponent>(uid) && !_mobStateSystem.IsAlive(uid))
                return;

            // How do we get trapped?
            // 1. Original target doesn't exist
            if (!component.OriginalEntity.IsValid() || Deleted(component.OriginalEntity))
            {
                GetTrapped(uid);
                return;
            }
            // 1. Original target is no longer mindswapped
            if (!TryComp<MindSwappedComponent>(component.OriginalEntity, out var targetMindSwap))
            {
                GetTrapped(uid);
                return;
            }

            // 2. Target has undergone a different mind swap
            if (targetMindSwap.OriginalEntity != uid)
            {
                GetTrapped(uid);
                return;
            }

            // 3. Target is dead
            if (HasComp<MobStateComponent>(component.OriginalEntity) && _mobStateSystem.IsDead(component.OriginalEntity))
            {
                GetTrapped(uid);
                return;
            }

            Swap(uid, component.OriginalEntity, true);
        }

        private void OnDispelled(EntityUid uid, MindSwappedComponent component, DispelledEvent args)
        {
            Swap(uid, component.OriginalEntity, true);
            args.Handled = true;
        }

        private void OnMobStateChanged(EntityUid uid, MindSwappedComponent component, MobStateChangedEvent args)
        {
            if (args.NewMobState == MobState.Dead)
                RemComp<MindSwappedComponent>(uid);
        }

        private void OnGhostAttempt(GhostAttemptHandleEvent args)
        {
            if (args.Handled)
                return;

            if (!HasComp<MindSwappedComponent>(args.Mind.CurrentEntity))
                return;

            //No idea where the viaCommand went. It's on the internal OnGhostAttempt, but not this layer. Maybe unnecessary.
            /*if (!args.viaCommand)
                return;*/

            args.Result = false;
            args.Handled = true;
        }

        private void OnSwapInit(EntityUid uid, MindSwappedComponent component, ComponentInit args)
        {
            _actions.AddAction(uid, ref component.MindSwapReturnActionEntity, component.MindSwapReturnActionId);
            _actions.TryGetActionData(component.MindSwapReturnActionEntity, out var actionData);
            if (actionData is { UseDelay: not null })
                _actions.StartUseDelay(component.MindSwapReturnActionEntity);
        }

        public void Swap(EntityUid performer, EntityUid target, bool end = false)
        {
            if (end && (!HasComp<MindSwappedComponent>(performer) || !HasComp<MindSwappedComponent>(target)))
                return;

            // Get the minds first. On transfer, they'll be gone.
            MindComponent? performerMind = null;
            MindComponent? targetMind = null;

            // This is here to prevent missing MindContainerComponent Resolve errors.
            if (!_mindSystem.TryGetMind(performer, out var performerMindId, out performerMind))
            {
                performerMind = null;
            };

            if (!_mindSystem.TryGetMind(target, out var targetMindId, out targetMind))
            {
                targetMind = null;
            };
            //This is a terrible way to 'unattach' minds. I wanted to use UnVisit but in TransferTo's code they say
            //To unnatch the minds, do it like this.
            //Have to unnattach the minds before we reattach them via transfer. Still feels weird, but seems to work well.
            _mindSystem.TransferTo(performerMindId, null);
            _mindSystem.TransferTo(targetMindId, null);
            // Do the transfer.
            if (performerMind != null)
                _mindSystem.TransferTo(performerMindId, target, ghostCheckOverride: true, false, performerMind);

            if (targetMind != null)
                _mindSystem.TransferTo(targetMindId, performer, ghostCheckOverride: true, false, targetMind);

            if (end)
            {
                var performerMindPowerComp = EntityManager.GetComponent<MindSwappedComponent>(performer);
                var targetMindPowerComp = EntityManager.GetComponent<MindSwappedComponent>(target);
                _actions.RemoveAction(performer, performerMindPowerComp.MindSwapReturnActionEntity);
                _actions.RemoveAction(target, targetMindPowerComp.MindSwapReturnActionEntity);

                RemComp<MindSwappedComponent>(performer);
                RemComp<MindSwappedComponent>(target);
                return;
            }

            var perfComp = EnsureComp<MindSwappedComponent>(performer);
            var targetComp = EnsureComp<MindSwappedComponent>(target);

            perfComp.OriginalEntity = target;
            targetComp.OriginalEntity = performer;
        }

        public void GetTrapped(EntityUid uid)
        {

            _popupSystem.PopupEntity(Loc.GetString("mindswap-trapped"), uid, uid, Shared.Popups.PopupType.LargeCaution);
            var perfComp = EnsureComp<MindSwappedComponent>(uid);
            _actions.RemoveAction(uid, perfComp.MindSwapReturnActionEntity, null);

            if (HasComp<TelegnosticProjectionComponent>(uid))
            {
                RemComp<PsionicallyInvisibleComponent>(uid);
                RemComp<StealthComponent>(uid);
                EnsureComp<SpeechComponent>(uid);
                EnsureComp<DispellableComponent>(uid);
                _metaDataSystem.SetEntityName(uid, Loc.GetString("telegnostic-trapped-entity-name"));
                _metaDataSystem.SetEntityDescription(uid, Loc.GetString("telegnostic-trapped-entity-desc"));
            }
        }
    }
}
