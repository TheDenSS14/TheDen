// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 ike709
// SPDX-FileCopyrightText: 2022 Jessica M
// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Rane
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Debug
// SPDX-FileCopyrightText: 2024 Whatstone
// SPDX-FileCopyrightText: 2025 AirFryerBuyOneGetOneFree
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Popups;
using Content.Shared.Stacks;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.Stack
{
    /// <summary>
    ///     Entity system that handles everything relating to stacks.
    ///     This is a good example for learning how to code in an ECS manner.
    /// </summary>
    [UsedImplicitly]
    public sealed class StackSystem : SharedStackSystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly SharedUserInterfaceSystem _ui = default!; // Cherry-picked from space-station-14#32938 courtesy of Ilya246

        public static readonly int[] DefaultSplitAmounts = { 1, 5, 10, 20, 30, 50 };

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<StackComponent, GetVerbsEvent<AlternativeVerb>>(OnStackAlternativeInteract);
        }

        public override void SetCount(EntityUid uid, int amount, StackComponent? component = null)
        {
            if (!Resolve(uid, ref component, false))
                return;

            base.SetCount(uid, amount, component);

            // Queue delete stack if count reaches zero.
            if (component.Count <= 0 && !component.Lingering)
                QueueDel(uid);
        }

        /// <summary>
        ///     Try to split this stack into two. Returns a non-null <see cref="Robust.Shared.GameObjects.EntityUid"/> if successful.
        /// </summary>
        public override EntityUid? Split(EntityUid uid, int amount, EntityCoordinates spawnPosition, StackComponent? stack = null) // Goobstation - override virtual method
        {
            if (!Resolve(uid, ref stack))
                return null;

            // Try to remove the amount of things we want to split from the original stack...
            if (!Use(uid, amount, stack))
                return null;

            // Get a prototype ID to spawn the new entity. Null is also valid, although it should rarely be picked...
            var prototype = _prototypeManager.TryIndex<StackPrototype>(stack.StackTypeId, out var stackType)
                ? stackType.Spawn
                : Prototype(uid)?.ID;

            // Set the output parameter in the event instance to the newly split stack.
            var entity = Spawn(prototype, spawnPosition);

            if (TryComp(entity, out StackComponent? stackComp))
            {
                // Set the split stack's count.
                SetCount(entity, amount, stackComp);
                // Don't let people dupe unlimited stacks
                stackComp.Unlimited = false;
            }

            var ev = new StackSplitEvent(entity);
            RaiseLocalEvent(uid, ref ev);

            return entity;
        }

        /// <summary>
        ///     Spawns a stack of a certain stack type. See <see cref="StackPrototype"/>.
        /// </summary>
        public EntityUid Spawn(int amount, StackPrototype prototype, EntityCoordinates spawnPosition)
        {
            // Set the output result parameter to the new stack entity...
            var entity = Spawn(prototype.Spawn, spawnPosition);
            var stack = Comp<StackComponent>(entity);

            // And finally, set the correct amount!
            SetCount(entity, amount, stack);
            return entity;
        }

        /// <summary>
        ///     Say you want to spawn 97 units of something that has a max stack count of 30.
        ///     This would spawn 3 stacks of 30 and 1 stack of 7.
        /// </summary>
        public List<EntityUid> SpawnMultiple(string entityPrototype, int amount, EntityCoordinates spawnPosition)
        {
            var proto = _prototypeManager.Index<EntityPrototype>(entityPrototype);
            proto.TryGetComponent<StackComponent>(out var stack);
            var maxCountPerStack = GetMaxCount(stack);
            var spawnedEnts = new List<EntityUid>();
            while (amount > 0)
            {
                var entity = Spawn(entityPrototype, spawnPosition);
                spawnedEnts.Add(entity);
                var countAmount = Math.Min(maxCountPerStack, amount);
                SetCount(entity, countAmount);
                amount -= countAmount;
            }
            return spawnedEnts;
        }

        private void OnStackAlternativeInteract(EntityUid uid, StackComponent stack, GetVerbsEvent<AlternativeVerb> args)
        {
            if (!args.CanAccess || !args.CanInteract || args.Hands == null || stack.Count == 1)
                return;

            // Frontier: cherry-picked from ss14#32938, moved up top
            var priority = 1;
            if (_ui.HasUi(uid, StackCustomSplitUiKey.Key)) // Frontier: check for interface
            {
                AlternativeVerb custom = new()
                {
                    Text = Loc.GetString("comp-stack-split-custom"),
                    Category = VerbCategory.Split,
                    Act = () =>
                    {
                        _ui.OpenUi(uid, StackCustomSplitUiKey.Key, args.User);
                    },
                    Priority = priority--
                };
                args.Verbs.Add(custom);
            }
            // End Frontier: cherry-picked from ss14#32938, moved up top

            AlternativeVerb halve = new()
            {
                Text = Loc.GetString("comp-stack-split-halve"),
                Category = VerbCategory.Split,
                Act = () => UserSplit(uid, args.User, stack.Count / 2, stack),
                Priority = priority-- // Frontier: 1<priority--
            };
            args.Verbs.Add(halve);

            foreach (var amount in DefaultSplitAmounts)
            {
                if (amount >= stack.Count)
                    continue;

                AlternativeVerb verb = new()
                {
                    Text = amount.ToString(),
                    Category = VerbCategory.Split,
                    Act = () => UserSplit(uid, args.User, amount, stack),
                    // we want to sort by size, not alphabetically by the verb text.
                    Priority = priority
                };

                priority--;

                args.Verbs.Add(verb);
            }
        }

        // Cherry-picked from ss14#32938 courtesy of Ilya246
        protected override void OnCustomSplitMessage(Entity<StackComponent> ent, ref StackCustomSplitAmountMessage message)
        {
            var (uid, comp) = ent;

            // digital ghosts shouldn't be allowed to split stacks
            if (!(message.Actor is { Valid: true } user))
                return;

            var amount = message.Amount;
            UserSplit(uid, user, amount, comp);
        }
        // End cherry-pick from ss14#32938 courtesy of Ilya246

        private void UserSplit(EntityUid uid, EntityUid userUid, int amount,
            StackComponent? stack = null,
            TransformComponent? userTransform = null)
        {
            if (!Resolve(uid, ref stack))
                return;

            if (!Resolve(userUid, ref userTransform))
                return;

            if (amount <= 0)
            {
                Popup.PopupCursor(Loc.GetString("comp-stack-split-too-small"), userUid, PopupType.Medium);
                return;
            }

            if (Split(uid, amount, userTransform.Coordinates, stack) is not {} split)
                return;

            Hands.PickupOrDrop(userUid, split);

            Popup.PopupCursor(Loc.GetString("comp-stack-split"), userUid);
        }
    }
}
