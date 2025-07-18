// SPDX-FileCopyrightText: 2022 Fishfish458 <47410468+Fishfish458@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 KIBORG04 <bossmira4@gmail.com>
// SPDX-FileCopyrightText: 2022 fishfish458 <fishfish458>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Engineering.Components;
using Content.Shared.DoAfter;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Verbs;
using JetBrains.Annotations;

namespace Content.Server.Engineering.EntitySystems
{
    [UsedImplicitly]
    public sealed class DisassembleOnAltVerbSystem : EntitySystem
    {
        [Dependency] private readonly SharedHandsSystem _handsSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<DisassembleOnAltVerbComponent, GetVerbsEvent<AlternativeVerb>>(AddDisassembleVerb);
        }
        private void AddDisassembleVerb(EntityUid uid, DisassembleOnAltVerbComponent component, GetVerbsEvent<AlternativeVerb> args)
        {
            if (!args.CanInteract || !args.CanAccess || args.Hands == null)
                return;

            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    AttemptDisassemble(uid, args.User, args.Target, component);
                },
                Text = Loc.GetString("disassemble-system-verb-disassemble"),
                Priority = 2
            };
            args.Verbs.Add(verb);
        }

        public async void AttemptDisassemble(EntityUid uid, EntityUid user, EntityUid target, DisassembleOnAltVerbComponent? component = null)
        {
            if (!Resolve(uid, ref component))
                return;
            if (string.IsNullOrEmpty(component.Prototype))
                return;

            if (component.DoAfterTime > 0 && TryGet<SharedDoAfterSystem>(out var doAfterSystem))
            {
                var doAfterArgs = new DoAfterArgs(EntityManager, user, component.DoAfterTime, new AwaitedDoAfterEvent(), null)
                {
                    BreakOnMove = true,
                };
                var result = await doAfterSystem.WaitDoAfter(doAfterArgs);

                if (result != DoAfterStatus.Finished)
                    return;
            }

            if (component.Deleted || Deleted(uid))
                return;

            if (!TryComp<TransformComponent>(uid, out var transformComp))
                return;

            var entity = EntityManager.SpawnEntity(component.Prototype, transformComp.Coordinates);

            _handsSystem.TryPickup(user, entity);

            EntityManager.DeleteEntity(uid);
        }
    }
}
