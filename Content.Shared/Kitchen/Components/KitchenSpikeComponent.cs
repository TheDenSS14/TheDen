// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Nutrition.Components;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Kitchen.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedKitchenSpikeSystem))]
public sealed partial class KitchenSpikeComponent : Component
{
    [DataField("delay")]
    public float SpikeDelay = 7.0f;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("sound")]
    public SoundSpecifier SpikeSound = new SoundPathSpecifier("/Audio/Effects/Fluids/splat.ogg");

    public List<string>? PrototypesToSpawn;

    // TODO: Spiking alive mobs? (Replace with uid) (deal damage to their limbs on spiking, kill on first butcher attempt?)
    [ViewVariables]
    public Container SpikeContainer = default!;
    // Note: This is not the "real" entity being spiked, but a "virtual entity"
    // containing only relevant components instead. I am pretty uncertain about the inherent
    // unpredictability of spiking entities who have a bunch of active systems running on them,
    // and I don't want to find out.

    // Prevents simultaneous spiking of two bodies (could be replaced with CancellationToken, but I don't see any situation where Cancel could be called)
    public bool InUse;

    [DataField]
    public string ContainerName = "spike";

    [Serializable, NetSerializable]
    public enum KitchenSpikeVisuals : byte
    {
        Status
    }

    [Serializable, NetSerializable]
    public enum KitchenSpikeStatus : byte
    {
        Empty,
        Bloody
    }
}
