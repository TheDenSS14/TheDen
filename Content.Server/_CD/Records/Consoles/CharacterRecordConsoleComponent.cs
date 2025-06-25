// SPDX-FileCopyrightText: 2025 Lyndomen <49795619+Lyndomen@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._CD.Records;
using Content.Shared.StationRecords;

namespace Content.Server._CD.Records.Consoles;

[RegisterComponent]
public sealed partial class CharacterRecordConsoleComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public uint? SelectedIndex { get; set; }

    [ViewVariables(VVAccess.ReadOnly)]
    public StationRecordsFilter? Filter;

    [DataField(required: true), ViewVariables(VVAccess.ReadOnly)]
    public RecordConsoleType ConsoleType;
}
