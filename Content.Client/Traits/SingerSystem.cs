// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Instruments;
using Content.Shared.Instruments;
using Content.Shared.Traits.Assorted.Prototypes;
using Content.Shared.Traits.Assorted.Systems;

namespace Content.Client.Traits;

public sealed class SingerSystem : SharedSingerSystem
{
    protected override SharedInstrumentComponent EnsureInstrumentComp(EntityUid uid, SingerInstrumentPrototype singer)
    {
        var instrumentComp = EnsureComp<InstrumentComponent>(uid);
        instrumentComp.AllowPercussion = singer.AllowPercussion;
        instrumentComp.AllowProgramChange = singer.AllowProgramChange;

        return instrumentComp;
    }
}
