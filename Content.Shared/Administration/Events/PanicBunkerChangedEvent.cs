// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Administration.Events;

[Serializable, NetSerializable]
public sealed class PanicBunkerStatus
{
    public bool Enabled;
    public bool DisableWithAdmins;
    public bool EnableWithoutAdmins;
    public bool CountDeadminnedAdmins;
    public bool ShowReason;
    public int MinAccountAgeHours;
    public int MinOverallHours;
}

[Serializable, NetSerializable]
public sealed class PanicBunkerChangedEvent : EntityEventArgs
{
    public PanicBunkerStatus Status;

    public PanicBunkerChangedEvent(PanicBunkerStatus status)
    {
        Status = status;
    }
}
