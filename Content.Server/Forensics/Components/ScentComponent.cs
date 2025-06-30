// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Forensics;

/// <summary>
/// This component is for mobs that have a Scent.
/// </summary>
[RegisterComponent]
public sealed partial class ScentComponent : Component
{
    [DataField]
    public string Scent = String.Empty;
}
