// SPDX-FileCopyrightText: 2024 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

/*
 * TODO: Remove baby jail code once a more mature gateway process is established. This code is only being issued as a stopgap to help with potential tiding in the immediate future.
 */

namespace Content.Shared.Administration.Events;

[Serializable, NetSerializable]
public sealed class BabyJailStatus
{
    public bool Enabled;
    public bool ShowReason;
    public int MaxAccountAgeHours;
    public int MaxOverallHours;
}

[Serializable, NetSerializable]
public sealed class BabyJailChangedEvent(BabyJailStatus status) : EntityEventArgs
{
    public BabyJailStatus Status = status;
}
