// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Access;

[Serializable, NetSerializable]
public enum AccessWireActionKey : byte
{
    Key,
    Status,
    Pulsed,
    PulseCancel
}

[Serializable, NetSerializable]
public enum LogWireActionKey : byte
{
    Key,
    Status,
    Pulsed,
    PulseCancel
}
