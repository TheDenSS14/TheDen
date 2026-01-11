// SPDX-FileCopyrightText: 2025 creku
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.DeviceLinking;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Explosion.Components
{
    /// <summary>
    /// Sends a device link signal when triggered.
    /// </summary>
    [RegisterComponent]
    public sealed partial class SignalOnTriggerComponent : Component
    {
        /// <summary>
        /// The port that gets signaled when the switch turns on.
        /// </summary>
        [DataField("port", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string Port = "Trigger";
    }
}