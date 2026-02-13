// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Humanoid.Markings;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Markings;

[Serializable]
public sealed class AnimatedToggleEvent : EntityEventArgs
{
    public EntityUid? ActionEntity;
    public ProtoId<MarkingPrototype> OldMarkingId;
    public ProtoId<MarkingPrototype> NewMarkingId;
}
