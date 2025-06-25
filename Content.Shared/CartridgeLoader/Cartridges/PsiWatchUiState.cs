// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Psionics;
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

/// <summary>
/// Show a list of wanted and suspected people from psionics records.
/// </summary>
[Serializable, NetSerializable]
public sealed class PsiWatchUiState : BoundUserInterfaceState
{
    public readonly List<PsiWatchEntry> Entries;

    public PsiWatchUiState(List<PsiWatchEntry> entries)
    {
        Entries = entries;
    }
}

/// <summary>
/// Entry for a person who is suspected, registered, or abusing.
/// </summary>
[Serializable, NetSerializable]
public record struct PsiWatchEntry(string Name, string Job, PsionicsStatus Status, string? Reason);
