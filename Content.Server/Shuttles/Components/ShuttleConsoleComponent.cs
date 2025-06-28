// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.Shuttles.Components;

namespace Content.Server.Shuttles.Components
{
    [RegisterComponent]
    public sealed partial class ShuttleConsoleComponent : SharedShuttleConsoleComponent
    {
        [ViewVariables]
        public readonly List<EntityUid> SubscribedPilots = new();

        /// <summary>
        /// How much should the pilot's eye be zoomed by when piloting using this console?
        /// </summary>
        [DataField("zoom")]
        public Vector2 Zoom = new(1.5f, 1.5f);

        /// <summary>
        /// Should this console have access to restricted FTL destinations?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("whitelistSpecific")]
        public List<EntityUid> FTLWhitelist = new List<EntityUid>();

        /// <summary>
        ///     How far the shuttle is allowed to jump(in meters).
        ///     TODO: This technically won't work until this component is migrated to Shared. The client console screen will only ever know the hardcoded 512 meter constant otherwise. Fix it in ShuttleMapControl.xaml.cs when that's done.
        /// </summary>
        [DataField]
        public float FTLRange = 512f;

        /// <summary>
        ///     If the shuttle is allowed to "Forcibly" land on planet surfaces, destroying anything it lands on. Used for SSTO capable shuttles.
        /// </summary>
        [DataField]
        public bool FtlToPlanets;

        /// <summary>
        ///     If the shuttle is allowed to forcibly land on stations, smimshing everything it lands on. This is where the hypothetical "Nukie drop pod" comes into play.
        /// </summary>
        [DataField]
        public bool IgnoreExclusionZones;

        /// <summary>
        ///     If the shuttle is only ever allowed to FTL once. Also used for the hypothetical "Nukie drop pod."
        /// </summary>
        [DataField]
        public bool OneWayTrip;

        /// <summary>
        ///     Tracks whether or not the above "One way trip" has been taken.
        /// </summary>
        public bool OneWayTripTaken;
    }
}
