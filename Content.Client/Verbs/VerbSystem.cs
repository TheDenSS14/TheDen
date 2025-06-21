// SPDX-FileCopyrightText: 2019 Silver <Silvertorch5@gmail.com>
// SPDX-FileCopyrightText: 2019 moneyl <8206401+Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 ColdAutumnRain <73938872+ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 DTanxxx <55208219+DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2020 R. Neuser <rneuser@iastate.edu>
// SPDX-FileCopyrightText: 2020 Tad Hardesty <tad@platymuus.com>
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 chairbender <kwhipke1@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Daniel Castro Razo <eldanielcr@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tornado Tech <54727692+Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 EctoplasmIsGood <109397347+EctoplasmIsGood@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Client.Examine;
using Content.Client.Gameplay;
using Content.Client.Popups;
using Content.Shared.Examine;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Client.ComponentTrees;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Shared.Map;
using Robust.Shared.Utility;

namespace Content.Client.Verbs
{
    [UsedImplicitly]
    public sealed class VerbSystem : SharedVerbSystem
    {
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly ExamineSystem _examine = default!;
        [Dependency] private readonly SpriteTreeSystem _tree = default!;
        [Dependency] private readonly TagSystem _tagSystem = default!;
        [Dependency] private readonly SharedTransformSystem _transform = default!;
        [Dependency] private readonly IStateManager _stateManager = default!;
        [Dependency] private readonly IEyeManager _eyeManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        /// <summary>
        ///     When a user right clicks somewhere, how large is the box we use to get entities for the context menu?
        /// </summary>
        public const float EntityMenuLookupSize = 0.25f;

        /// <summary>
        ///     These flags determine what entities the user can see on the context menu.
        /// </summary>
        public MenuVisibility Visibility;

        public Action<VerbsResponseEvent>? OnVerbsResponse;

        private List<EntityUid> _entities = new();

        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<VerbsResponseEvent>(HandleVerbResponse);
        }

        /// <summary>
        ///     Get all of the entities in an area for displaying on the context menu.
        /// </summary>
        public bool TryGetEntityMenuEntities(MapCoordinates targetPos, [NotNullWhen(true)] out List<EntityUid>? result)
        {
            result = null;

            if (_stateManager.CurrentState is not GameplayStateBase gameScreenBase)
                return false;

            var player = _playerManager.LocalEntity;
            if (player == null)
                return false;

            // If FOV drawing is disabled, we will modify the visibility option to ignore visiblity checks.
            var visibility = _eyeManager.CurrentEye.DrawFov
                ? Visibility
                : Visibility | MenuVisibility.NoFov;

            var ev = new MenuVisibilityEvent()
            {
                TargetPos = targetPos,
                Visibility = visibility,
            };

            RaiseLocalEvent(player.Value, ref ev);
            visibility = ev.Visibility;

            // Get entities
            _entities.Clear();
            var entitiesUnderMouse = _tree.QueryAabb(targetPos.MapId, Box2.CenteredAround(targetPos.Position, new Vector2(EntityMenuLookupSize, EntityMenuLookupSize)));

            // Do we have to do FoV checks?
            if ((visibility & MenuVisibility.NoFov) == 0)
            {
                bool Predicate(EntityUid e) => e == player;

                TryComp(player.Value, out ExaminerComponent? examiner);

                foreach (var ent in entitiesUnderMouse)
                {
                    if (_examine.CanExamine(player.Value, targetPos, Predicate, ent.Uid, examiner))
                        _entities.Add(ent.Uid);
                }
            }
            else
            {
                foreach (var ent in entitiesUnderMouse)
                {
                    _entities.Add(ent.Uid);
                }
            }

            if (_entities.Count == 0)
                return false;

            if (visibility == MenuVisibility.All)
            {
                result = new (_entities);
                return true;
            }

            // remove any entities in containers
            if ((visibility & MenuVisibility.InContainer) == 0)
            {
                for (var i = _entities.Count - 1; i >= 0; i--)
                {
                    var entity = _entities[i];

                    if (ContainerSystem.IsInSameOrTransparentContainer(player.Value, entity))
                        continue;

                    _entities.RemoveSwap(i);
                }
            }

            // remove any invisible entities
            if ((visibility & MenuVisibility.Invisible) == 0)
            {
                var spriteQuery = GetEntityQuery<SpriteComponent>();

                for (var i = _entities.Count - 1; i >= 0; i--)
                {
                    var entity = _entities[i];

                    if (!spriteQuery.TryGetComponent(entity, out var spriteComponent) ||
                        !spriteComponent.Visible ||
                        _tagSystem.HasTag(entity, "HideContextMenu"))
                    {
                        _entities.RemoveSwap(i);
                    }
                }
            }

            if (_entities.Count == 0)
                return false;

            result = new(_entities);
            return true;
        }

        /// <summary>
        ///     Ask the server to send back a list of server-side verbs, and for now return an incomplete list of verbs
        ///     (only those defined locally).
        /// </summary>
        public SortedSet<Verb> GetVerbs(NetEntity target, EntityUid user, List<Type> verbTypes, out List<VerbCategory> extraCategories, bool force = false)
        {
            if (!target.IsClientSide())
                RaiseNetworkEvent(new RequestServerVerbsEvent(target, verbTypes, adminRequest: force));

            // Some admin menu interactions will try get verbs for entities that have not yet been sent to the player.
            if (!TryGetEntity(target, out var local))
            {
                extraCategories = new();
                return new();
            }

            return GetLocalVerbs(local.Value, user, verbTypes, out extraCategories, force);
        }


        /// <summary>
        ///     Execute actions associated with the given verb.
        /// </summary>
        /// <remarks>
        ///     Unless this is a client-exclusive verb, this will also tell the server to run the same verb.
        /// </remarks>
        public void ExecuteVerb(EntityUid target, Verb verb)
        {
            ExecuteVerb(GetNetEntity(target), verb);
        }

        /// <summary>
        ///     Execute actions associated with the given verb.
        /// </summary>
        /// <remarks>
        ///     Unless this is a client-exclusive verb, this will also tell the server to run the same verb.
        /// </remarks>
        public void ExecuteVerb(NetEntity target, Verb verb)
        {
            if ( _playerManager.LocalEntity is not {} user)
                return;

            // is this verb actually valid?
            if (verb.Disabled)
            {
                // maybe send an informative pop-up message.
                if (!string.IsNullOrWhiteSpace(verb.Message))
                    _popupSystem.PopupEntity(verb.Message, user);

                return;
            }

            if (verb.ClientExclusive || target.IsClientSide())
                // is this a client exclusive (gui) verb?
                ExecuteVerb(verb, user, GetEntity(target));
            else
                EntityManager.RaisePredictiveEvent(new ExecuteVerbEvent(target, verb));
        }

        private void HandleVerbResponse(VerbsResponseEvent msg)
        {
            OnVerbsResponse?.Invoke(msg);
        }
    }
}
