// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 theashtronaut <112137107+theashtronaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 HerCoyote23 <131214189+HerCoyote23@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 c4llv07e <38111072+c4llv07e@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared.UserInterface
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ActivatableUIComponent : Component
    {
        [DataField(required: true, customTypeSerializer: typeof(EnumSerializer))]
        public Enum? Key;

        /// <summary>
        /// Whether the item must be held in one of the user's hands to work.
        /// This is ignored unless <see cref="RequiresComplex"/> is true.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool InHandsOnly;

        [DataField]
        public bool SingleUser;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool AdminOnly;

        [DataField]
        public LocId VerbText = "ui-verb-toggle-open";

        /// <summary>
        ///     Whether you need to be able to do complex interactions to operate this UI.
        /// </summary>
        /// <remarks>
        ///     This should probably be true for most machines & computers, but there will still be UIs that represent a
        ///     more generic interaction / configuration that might not require complex.
        /// </remarks>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool RequiresComplex = true;

        /// <summary>
        ///     Entities that are required to open this UI.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public EntityWhitelist? RequiredItems;

        /// <summary>
        ///     Whitelist for the user who is trying to open this UI.
        /// </summary>
        [DataField]
        public EntityWhitelist? UserWhitelist;

        /// <summary>
        ///     If true, then this UI can only be opened via verbs. I.e., normal interactions/activations will not open
        ///     the UI.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool VerbOnly;

        /// <summary>
        ///     Goobstation
        ///     If true, UI can only be opened via alt verb.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool AltVerb;

        /// <summary>
        ///     Whether spectators (non-admin ghosts) should be allowed to view this UI.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool BlockSpectators;

        /// <summary>
        ///     Whether the item must be in the user's currently selected/active hand.
        ///     This is ignored unless <see cref="InHandsOnly"/> is true.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool RequireActiveHand = true;

        /// <summary>
        ///     The client channel currently using the object, or null if there's none/not single user.
        ///     NOTE: DO NOT DIRECTLY SET, USE ActivatableUISystem.SetCurrentSingleUser
        /// </summary>
        [DataField, AutoNetworkedField]
        public EntityUid? CurrentSingleUser;
    }
}
