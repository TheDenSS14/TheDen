// SPDX-FileCopyrightText: 2025 neuPanda <chriseparton@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

// NeuPanda - This file is licensed under AGPLv3
// Copyright (c) 2025 NeuPanda
// See AGPLv3.txt for details.
using Robust.Shared.Serialization;

namespace Content.Shared._NF.Shuttles.Events
{
    /// <summary>
    /// Raised on the client when it wishes to not have 2 docking ports docked.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class SetInertiaDampeningRequest : BoundUserInterfaceMessage
    {
        public NetEntity? ShuttleEntityUid { get; set; }
        public InertiaDampeningMode Mode { get; set; }
    }

    [Serializable, NetSerializable]
    public enum InertiaDampeningMode
    {
        Off = 0,
        Dampened = 1,
        Anchored = 2
    }
}
