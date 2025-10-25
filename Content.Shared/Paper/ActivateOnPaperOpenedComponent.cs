// SPDX-FileCopyrightText: 2024 Plykiya
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Paper;

namespace Content.Shared.Paper;

/// <summary>
/// Activates the item when used to write on paper, as if Z was pressed.
/// </summary>
[RegisterComponent]
[Access(typeof(PaperSystem))]
public sealed partial class ActivateOnPaperOpenedComponent : Component
{
}
