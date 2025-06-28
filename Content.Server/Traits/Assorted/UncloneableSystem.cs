// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Cloning;

namespace Content.Server.Traits.Assorted
{
    public sealed class UncloneableSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<UncloneableComponent, AttemptCloningEvent>(OnAttemptCloning);
        }

        private void OnAttemptCloning(EntityUid uid, UncloneableComponent component, ref AttemptCloningEvent args)
        {
            if (args.CloningFailMessage is not null
                || args.Cancelled)
                return;

            args.CloningFailMessage = "cloning-console-uncloneable-trait-error";
            args.Cancelled = true;
        }
    }
}
