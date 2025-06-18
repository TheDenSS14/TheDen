// SPDX-FileCopyrightText: 2021 FoLoKe <36813380+FoLoKe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Scribbles0 <91828755+Scribbles0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 M3739 <47579354+M3739@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Body.Systems;
using Content.Server.Kitchen.Components;
using Content.Server.Popups;
using Content.Server.Temperature.Components;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Chat;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.DragDrop;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Kitchen;
using Content.Shared.Kitchen.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;
using static Content.Shared.Kitchen.Components.KitchenSpikeComponent;

namespace Content.Server.Kitchen.EntitySystems
{
    public sealed class KitchenSpikeSystem : SharedKitchenSpikeSystem
    {
        [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
        [Dependency] private readonly IAdminLogManager _logger = default!;
        [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly TransformSystem _transform = default!;
        [Dependency] private readonly BodySystem _bodySystem = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly MetaDataSystem _metaData = default!;
        [Dependency] private readonly SharedSuicideSystem _suicide = default!;
        [Dependency] private readonly ISerializationManager _serMan = default!;
        [Dependency] private readonly SharedRottingSystem _rotting = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<KitchenSpikeComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<KitchenSpikeComponent, ComponentShutdown>(OnRemove);
            SubscribeLocalEvent<KitchenSpikeComponent, InteractUsingEvent>(OnInteractUsing);
            SubscribeLocalEvent<KitchenSpikeComponent, InteractHandEvent>(OnInteractHand);
            SubscribeLocalEvent<KitchenSpikeComponent, DragDropTargetEvent>(OnDragDrop);

            //DoAfter
            SubscribeLocalEvent<KitchenSpikeComponent, SpikeDoAfterEvent>(OnDoAfter);

            SubscribeLocalEvent<KitchenSpikeComponent, SuicideByEnvironmentEvent>(OnSuicideByEnvironment);
            SubscribeLocalEvent<KitchenSpikeComponent, ExaminedEvent>(OnExamined);

            SubscribeLocalEvent<ButcherableComponent, CanDropDraggedEvent>(OnButcherableCanDrop);
        }

        private void OnInit(EntityUid uid, KitchenSpikeComponent spike, ComponentInit args)
        {
            var cont = _containerSystem.EnsureContainer<Container>(uid, spike.ContainerName);
            spike.SpikeContainer = cont;
        }

        private void OnRemove(EntityUid uid, KitchenSpikeComponent spike, ComponentShutdown args)
        {
            if (spike.SpikeContainer != null)
                _containerSystem.CleanContainer(spike.SpikeContainer);
        }

        private void OnButcherableCanDrop(Entity<ButcherableComponent> entity, ref CanDropDraggedEvent args)
        {
            args.Handled = true;
            args.CanDrop |= entity.Comp.Type != ButcheringType.Knife;
        }

        /// <summary>
        /// TODO: Update this so it actually meatspikes the user instead of applying lethal damage to them.
        /// </summary>
        private void OnSuicideByEnvironment(Entity<KitchenSpikeComponent> entity, ref SuicideByEnvironmentEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp<DamageableComponent>(args.Victim, out var damageableComponent))
                return;

            _suicide.ApplyLethalDamage((args.Victim, damageableComponent), "Piercing");
            var othersMessage = Loc.GetString("comp-kitchen-spike-suicide-other", ("victim", args.Victim));
            _popupSystem.PopupEntity(othersMessage, args.Victim, Filter.PvsExcept(args.Victim), true);

            var selfMessage = Loc.GetString("comp-kitchen-spike-suicide-self");
            _popupSystem.PopupEntity(selfMessage, args.Victim, args.Victim);
            args.Handled = true;
        }

        private void OnDoAfter(Entity<KitchenSpikeComponent> entity, ref SpikeDoAfterEvent args)
        {
            if (args.Args.Target == null)
                return;

            if (TryComp<ButcherableComponent>(args.Args.Target.Value, out var butcherable))
                butcherable.BeingButchered = false;

            if (args.Cancelled)
            {
                entity.Comp.InUse = false;
                return;
            }

            if (args.Handled)
                return;

            if (Spikeable(entity, args.Args.User, args.Args.Target.Value, entity.Comp, butcherable))
                Spike(entity, args.Args.User, args.Args.Target.Value, entity.Comp);

            entity.Comp.InUse = false;
            args.Handled = true;
        }

        private void OnDragDrop(Entity<KitchenSpikeComponent> entity, ref DragDropTargetEvent args)
        {
            if (args.Handled)
                return;

            args.Handled = true;

            if (Spikeable(entity, args.User, args.Dragged, entity.Comp))
                TrySpike(entity, args.User, args.Dragged, entity.Comp);
        }

        private void OnInteractHand(Entity<KitchenSpikeComponent> entity, ref InteractHandEvent args)
        {
            if (args.Handled)
                return;

            if (entity.Comp.PrototypesToSpawn?.Count > 0)
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-knife-needed"), entity, args.User);
                args.Handled = true;
            }
        }

        private void OnInteractUsing(Entity<KitchenSpikeComponent> entity, ref InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (TryGetPiece(entity, args.User, args.Used))
                args.Handled = true;
        }

        private void OnExamined(Entity<KitchenSpikeComponent> spike, ref ExaminedEvent args)
        {
            var comp = spike.Comp;
            var hasSpiked = TryGetSpikedEntity(spike, comp, out var spiked);
            if (!hasSpiked)
                return;

            if (TryComp<PerishableComponent>(spiked, out var perishable))
            {
                var ent = new Entity<PerishableComponent>(spiked.Value, perishable);
                var examineText = _rotting.GetPerishableExamineText(ent);
                if (examineText != string.Empty)
                    args.PushMarkup(examineText);
            }

            if (TryComp<RottingComponent>(spiked, out var rotting))
            {
                var ent = new Entity<RottingComponent>(spiked.Value, rotting);
                var examineText = _rotting.GetRottingExamineText(ent);
                if (examineText != string.Empty)
                    args.PushMarkup(examineText);
            }
        }

        private void Spike(EntityUid uid, EntityUid userUid, EntityUid victimUid,
            KitchenSpikeComponent? component = null, ButcherableComponent? butcherable = null)
        {
            if (!Resolve(uid, ref component) || !Resolve(victimUid, ref butcherable))
                return;

            _logger.Add(LogType.Gib, LogImpact.Extreme, $"{ToPrettyString(userUid):user} kitchen spiked {ToPrettyString(victimUid):target}");

            // TODO VERY SUS
            var virtualEnt = CreateVirtualSpikedEntity(uid, victimUid);
            _containerSystem.Insert(virtualEnt, component.SpikeContainer);
            component.PrototypesToSpawn = EntitySpawnCollection.GetSpawns(butcherable.SpawnedEntities, _random);
            UpdateAppearance(uid, null, component);

            _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-kill", ("user", Identity.Entity(userUid, EntityManager)), ("victim", victimUid)), uid, PopupType.LargeCaution);

            _transform.SetCoordinates(victimUid, Transform(uid).Coordinates);
            // THE WHAT?
            // TODO: Need to be able to leave them on the spike to do DoT, see ss13.
            var gibs = _bodySystem.GibBody(victimUid); // DeltaV: spawn organs
            foreach (var gib in gibs)
                QueueDel(gib);

            _audio.PlayEntity(component.SpikeSound, Filter.Pvs(uid), uid, true);
        }

        /// <summary>
        /// Creates a "basic" representation of a butchered entity using only components
        /// that are relevant to meat spiking. TODO storing the real entity on the spike and making
        /// sure it doesn't suck when we do
        /// </summary>
        /// <param name="spikeId">The ID of the spike entity.</param>
        /// <param name="victimId">The ID of the entity being spiked.</param>
        /// <returns>The ID of the new "virtual" enttity.</returns>
        private EntityUid CreateVirtualSpikedEntity(EntityUid spikeId, EntityUid victimId)
        {
            var ent = Spawn(null, Transform(spikeId).Coordinates);

            var meta = EnsureComp<MetaDataComponent>(ent);
            if (TryComp(victimId, out MetaDataComponent? victimMeta))
                _metaData.SetEntityName(ent, victimMeta.EntityName, meta);

            CopyComponent<MobStateComponent>(victimId, ent);
            CopyComponent<ButcherableComponent>(victimId, ent);
            CopyComponent<PerishableComponent>(victimId, ent);
            CopyComponent<RottingComponent>(victimId, ent);
            CopyComponent<TemperatureComponent>(victimId, ent);
            return ent;
        }

        // TODO: move this somewhere that isn't KitchenSpikeSystem
        private void CopyComponent<T>(EntityUid src, EntityUid target) where T : Component, new()
        {
            if (!TryComp<T>(src, out var srcComp))
                return;

            var targetComp = EnsureComp<T>(target);
            _serMan.CopyTo(srcComp, ref targetComp, notNullableOverride: true);
        }

        private bool TryGetPiece(EntityUid uid, EntityUid user, EntityUid used,
            KitchenSpikeComponent? component = null, SharpComponent? sharp = null)
        {
            if (!Resolve(uid, ref component)
            || component.PrototypesToSpawn == null
            || component.PrototypesToSpawn.Count == 0
            || !TryGetSpikedEntity(uid, component, out var spiked))
                return false;

            // Is using knife
            if (!Resolve(used, ref sharp, false))
            {
                return false;
            }

            var item = _random.PickAndTake(component.PrototypesToSpawn);
            var ent = Spawn(item, Transform(uid).Coordinates);
            _metaData.SetEntityName(ent,
                Loc.GetString("comp-kitchen-spike-meat-name",
                    ("name", Name(ent)),
                    ("victim", Name(spiked.Value))));

            _rotting.TransferFreshness(spiked.Value, ent, true);
            _rotting.TransferRotStage(spiked.Value, ent, true);

            if (component.PrototypesToSpawn.Count != 0)
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-remove-meat",
                    ("victim", spiked)),
                    uid,
                    user,
                    PopupType.MediumCaution);
            else
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-remove-meat-last",
                    ("victim", spiked)),
                    uid,
                    user,
                    PopupType.MediumCaution);

                RemoveSpikedEntity(uid, component);
                UpdateAppearance(uid, null, component);
            }

            return true;
        }

        private bool TryGetSpikedEntity(EntityUid uid,
            KitchenSpikeComponent? component,
            [NotNullWhen(true)] out EntityUid? spikedEntity)
        {
            spikedEntity = null;
            if (!Resolve(uid, ref component)
                || component.SpikeContainer.Count == 0)
                return false;

            spikedEntity = component.SpikeContainer.ContainedEntities.First();
            return spikedEntity != null;
        }

        public void RemoveSpikedEntity(EntityUid uid, KitchenSpikeComponent comp)
        {
            if (TryGetSpikedEntity(uid, comp, out var spiked))
            {
                QueueDel(spiked);
                _containerSystem.CleanContainer(comp.SpikeContainer);
            }
        }

        private void UpdateAppearance(EntityUid uid, AppearanceComponent? appearance = null, KitchenSpikeComponent? component = null)
        {
            if (!Resolve(uid, ref component, ref appearance, false))
                return;

            _appearance.SetData(uid,
                KitchenSpikeVisuals.Status,
                component.SpikeContainer.Count > 0 ? KitchenSpikeStatus.Bloody : KitchenSpikeStatus.Empty,
                appearance);
        }

        private bool Spikeable(EntityUid uid, EntityUid userUid, EntityUid victimUid,
            KitchenSpikeComponent? component = null, ButcherableComponent? butcherable = null)
        {
            if (!Resolve(uid, ref component))
                return false;

            if (component.PrototypesToSpawn?.Count > 0
                || component.SpikeContainer?.ContainedEntities.Count > 0)
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-deny-collect", ("this", uid)), uid, userUid);
                return false;
            }

            if (!Resolve(victimUid, ref butcherable, false))
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-deny-butcher", ("victim", Identity.Entity(victimUid, EntityManager)), ("this", uid)), victimUid, userUid);
                return false;
            }

            switch (butcherable.Type)
            {
                case ButcheringType.Spike:
                    return true;
                case ButcheringType.Knife:
                    _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-deny-butcher-knife", ("victim", Identity.Entity(victimUid, EntityManager)), ("this", uid)), victimUid, userUid);
                    return false;
                default:
                    _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-deny-butcher", ("victim", Identity.Entity(victimUid, EntityManager)), ("this", uid)), victimUid, userUid);
                    return false;
            }
        }

        public bool TrySpike(EntityUid uid, EntityUid userUid, EntityUid victimUid, KitchenSpikeComponent? component = null,
            ButcherableComponent? butcherable = null, MobStateComponent? mobState = null)
        {
            if (!Resolve(uid, ref component) || component.InUse ||
                !Resolve(victimUid, ref butcherable) || butcherable.BeingButchered)
                return false;

            // THE WHAT? (again)
            // Prevent dead from being spiked TODO: Maybe remove when rounds can be played and DOT is implemented
            if (Resolve(victimUid, ref mobState, false) &&
                _mobStateSystem.IsAlive(victimUid, mobState))
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-deny-not-dead", ("victim", Identity.Entity(victimUid, EntityManager))),
                    victimUid, userUid);
                return true;
            }

            if (userUid != victimUid)
            {
                _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-begin-hook-victim", ("user", Identity.Entity(userUid, EntityManager)), ("this", uid)), victimUid, victimUid, PopupType.LargeCaution);
            }
            // TODO: make it work when SuicideEvent is implemented
            // else
            //    _popupSystem.PopupEntity(Loc.GetString("comp-kitchen-spike-begin-hook-self", ("this", uid)), victimUid, Filter.Pvs(uid)); // This is actually unreachable and should be in SuicideEvent

            butcherable.BeingButchered = true;
            component.InUse = true;

            var doAfterArgs = new DoAfterArgs(EntityManager, userUid, component.SpikeDelay + butcherable.ButcherDelay, new SpikeDoAfterEvent(), uid, target: victimUid, used: uid)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                NeedHand = true
            };

            _doAfter.TryStartDoAfter(doAfterArgs);

            return true;
        }
    }
}
