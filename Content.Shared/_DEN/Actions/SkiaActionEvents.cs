// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Jakumba
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Actions;


public sealed partial class RerollSkiaObjectivesActionEvent : InstantActionEvent;
public sealed partial class SkiaShopActionEvent : InstantActionEvent;
public sealed partial class SkiaSpawnMobsActionEvent : InstantActionEvent;

[Serializable, NetSerializable]
public sealed partial class SkiaReapingEvent : SimpleDoAfterEvent { }
