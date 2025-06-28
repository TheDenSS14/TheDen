// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Soul
{
    /// <summary>
    /// Key representing which <see cref="BoundUserInterface"/> is currently open.
    /// Useful when there are multiple UI for an object. Here it's future-proofing only.
    /// </summary>
    [Serializable, NetSerializable]
    public enum GolemUiKey : byte
    {
        Key,
    }

    /// <summary>
    /// Represents an <see cref="GolemComponent"/> state that can be sent to the client
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class GolemBoundUserInterfaceState : BoundUserInterfaceState
    {
        public string Name { get; }
        public string MasterName { get; }

        public GolemBoundUserInterfaceState(string name, string currentMasterName)
        {
            Name = name;
            MasterName = currentMasterName;
        }
    }

    [Serializable, NetSerializable]
    public sealed class GolemNameChangedMessage : BoundUserInterfaceMessage
    {
        public string Name { get; }

        public GolemNameChangedMessage(string name)
        {
            Name = name;
        }
    }

    [Serializable, NetSerializable]
    public sealed class GolemMasterNameChangedMessage : BoundUserInterfaceMessage
    {
        public string MasterName { get; }
        public GolemMasterNameChangedMessage(string masterName)
        {
            MasterName = masterName;
        }
    }

    /// <summary>
    ///     Install this golem!!!!
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class GolemInstallRequestMessage : BoundUserInterfaceMessage
    {
        public GolemInstallRequestMessage()
        {}
    }
}
