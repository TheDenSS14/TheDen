// SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LordEclipse <106132477+LordEclipse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Forensics
{
    /// <summary>
    /// This component is for mobs that leave fingerprints.
    /// </summary>
    [RegisterComponent]
    public sealed partial class FingerprintComponent : Component
    {
        [DataField("fingerprint"), ViewVariables(VVAccess.ReadWrite)]
        public string? Fingerprint;
    }
}
