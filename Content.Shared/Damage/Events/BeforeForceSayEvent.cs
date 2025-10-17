// SPDX-FileCopyrightText: 2025 bigsantino1
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Damage.Events;

/// <summary>
///     Event for interrupting and changing the prefix for when an entity is being forced to say something
/// </summary>
[Serializable, NetSerializable]
public sealed class BeforeForceSayEvent(ProtoId<LocalizedDatasetPrototype> prefixDataset) : EntityEventArgs
{
    public ProtoId<LocalizedDatasetPrototype> Prefix = prefixDataset;
}
