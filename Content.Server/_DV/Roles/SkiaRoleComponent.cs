// SPDX-FileCopyrightText: 2025 William Lemon
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Roles;

namespace Content.Shared._DV.Roles;

/// <summary>
/// Added to mind role entities to tag that they are a Skia.
/// </summary>
[RegisterComponent]
public sealed partial class SkiaRoleComponent : BaseMindRoleComponent;
