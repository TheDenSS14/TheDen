// SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 faint <46868845+ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Forensics
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ForensicsComponent : Component
    {
        [DataField, AutoNetworkedField]
        public HashSet<string> Fingerprints = new();

        [DataField, AutoNetworkedField]
        public HashSet<string> Fibers = new();

        [DataField, AutoNetworkedField]
        public HashSet<string> DNAs = new();

        [DataField, AutoNetworkedField]
        public string Scent = String.Empty;

        [DataField, AutoNetworkedField]
        public HashSet<string> Residues = new();

        /// <summary>
        /// How close you must be to wipe the prints/blood/etc. off of this entity
        /// </summary>
        [DataField("cleanDistance")]
        public float CleanDistance = 1.5f;

        /// <summary>
        /// Can the DNA be cleaned off of this entity?
        /// e.g. you can wipe the DNA off of a knife, but not a cigarette
        /// </summary>
        [DataField("canDnaBeCleaned")]
        public bool CanDnaBeCleaned = true;

        /// <summary>
        /// Moment in time next effect will be spawned
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan TargetTime = TimeSpan.Zero;
    }
}
