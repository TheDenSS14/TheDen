// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;

namespace Content.Shared.HeightAdjust;

/// <summary>
///     Raised on a humanoid after their scale has been adjusted in accordance with their profile and their physics have been updated.
/// </summary>
public sealed class HeightAdjustedEvent : EntityEventArgs
{
    public Vector2 NewScale;
}
