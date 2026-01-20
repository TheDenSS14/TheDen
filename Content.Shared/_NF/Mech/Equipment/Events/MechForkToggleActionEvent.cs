// SPDX-FileCopyrightText: 2025 Whatstone
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Actions;

namespace Content.Shared._NF.Mech.Equipment.Events;

/// <summary>
/// An action raised on a power fork that the direction should change
/// </summary>
public sealed partial class MechForkToggleActionEvent : InstantActionEvent;
