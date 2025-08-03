// SPDX-FileCopyrightText: 2024 LankLTE
// SPDX-FileCopyrightText: 2025 Falcon
// SPDX-FileCopyrightText: 2025 foxcurl
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._CD.Traits;

/// <summary>
/// Set players' blood to coolant, and is used to notify them of ion storms
/// </summary>
[RegisterComponent, Access(typeof(SynthSystem))]
public sealed partial class SynthComponent : Component { }
