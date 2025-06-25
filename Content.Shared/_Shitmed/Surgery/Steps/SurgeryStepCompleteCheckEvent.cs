// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[ByRefEvent]
public record struct SurgeryStepCompleteCheckEvent(EntityUid Body, EntityUid Part, EntityUid Surgery, bool Cancelled = false);