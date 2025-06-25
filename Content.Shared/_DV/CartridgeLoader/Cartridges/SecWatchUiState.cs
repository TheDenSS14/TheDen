// SPDX-FileCopyrightText: 2024 Fansana <116083121+Fansana@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Security;
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

/// <summary>
/// Show a list of wanted and suspected people from criminal records.
/// </summary>
[Serializable, NetSerializable]
public sealed class SecWatchUiState : BoundUserInterfaceState
{
    public readonly List<SecWatchEntry> Entries;

    public SecWatchUiState(List<SecWatchEntry> entries)
    {
        Entries = entries;
    }
}

/// <summary>
/// Entry for a person who is wanted or suspected.
/// </summary>
[Serializable, NetSerializable]
public record struct SecWatchEntry(string Name, string Job, SecurityStatus Status, string? Reason);
