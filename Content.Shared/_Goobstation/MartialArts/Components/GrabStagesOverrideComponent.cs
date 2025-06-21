// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Movement.Pulling.Systems;

namespace Content.Shared._Goobstation.MartialArts.Components;

/// <summary>
/// Base component for martial arts that override the normal grab stages.
/// Allows martial arts to start at more advanced grab stages like Hard grabs.
/// </summary>
public abstract partial class GrabStagesOverrideComponent : Component
{
    public GrabStage StartingStage = GrabStage.Hard;
}
