// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 faint <46868845+ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Forensics
{
    [Serializable, NetSerializable]
    public sealed class ForensicScannerBoundUserInterfaceState : BoundUserInterfaceState
    {
        public readonly List<string> Fingerprints = new();
        public readonly List<string> Fibers = new();
        public readonly List<string> DNAs = new();
        public readonly List<string> Residues = new();
        public readonly string LastScannedName = string.Empty;
        public readonly TimeSpan PrintCooldown = TimeSpan.Zero;
        public readonly TimeSpan PrintReadyAt = TimeSpan.Zero;

        public ForensicScannerBoundUserInterfaceState(
            List<string> fingerprints,
            List<string> fibers,
            List<string> dnas,
            List<string> residues,
            string lastScannedName,
            TimeSpan printCooldown,
            TimeSpan printReadyAt)
        {
            Fingerprints = fingerprints;
            Fibers = fibers;
            DNAs = dnas;
            Residues = residues;
            LastScannedName = lastScannedName;
            PrintCooldown = printCooldown;
            PrintReadyAt = printReadyAt;
        }
    }

    [Serializable, NetSerializable]
    public enum ForensicScannerUiKey : byte
    {
        Key
    }

    [Serializable, NetSerializable]
    public sealed class ForensicScannerPrintMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class ForensicScannerClearMessage : BoundUserInterfaceMessage
    {
    }
}
