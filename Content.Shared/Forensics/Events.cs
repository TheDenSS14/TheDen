// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Forensics;

[Serializable, NetSerializable]
public sealed partial class ForensicScannerDoAfterEvent : SimpleDoAfterEvent
{
}

[Serializable, NetSerializable]
public sealed partial class ForensicPadDoAfterEvent : DoAfterEvent
{
    [DataField("sample", required: true)] public string Sample = default!;

    private ForensicPadDoAfterEvent()
    {
    }

    public ForensicPadDoAfterEvent(string sample)
    {
        Sample = sample;
    }

    public override DoAfterEvent Clone() => this;
}

[Serializable, NetSerializable]
public sealed partial class ScentTrackerDoAfterEvent : SimpleDoAfterEvent { }

[Serializable, NetSerializable]
public sealed partial class CleanForensicsDoAfterEvent : SimpleDoAfterEvent
{
}

/// <summary>
/// An event to apply DNA evidence from a donor onto some recipient.
/// </summary>
[ByRefEvent]
public record struct TransferDnaEvent()
{
    /// <summary>
    /// The entity donating the DNA.
    /// </summary>
    public EntityUid Donor;

    /// <summary>
    /// The entity receiving the DNA.
    /// </summary>
    public EntityUid Recipient;

    /// <summary>
    /// Can the DNA be cleaned off?
    /// </summary>
    public bool CanDnaBeCleaned = true;
}
