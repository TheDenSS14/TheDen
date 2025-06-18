// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Instruments;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits.Assorted.Prototypes;

[Prototype("SingerInstrument")]
public sealed partial class SingerInstrumentPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Configuration for SwappableInstrumentComponent.
    ///     string = display name of the instrument
    ///     byte 1 = instrument midi program
    ///     byte 2 = instrument midi bank
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, (byte, byte)> InstrumentList = new();

    /// <summary>
    ///     Instrument in <see cref="InstrumentList"/> that is used by default.
    /// </summary>
    [DataField(required: true)]
    public string DefaultInstrument = string.Empty;

    /// <summary>
    ///     The BUI configuration for the instrument.
    /// </summary>
    [DataField]
    public InstrumentUiKey? MidiUi;

    // The below is server only, as it uses a server-BUI event !type
    [DataField(serverOnly: true, required: true)]
    public EntProtoId MidiActionId;

    [DataField]
    public bool AllowPercussion;

    [DataField]
    public bool AllowProgramChange;
}
