// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

namespace Content.Server.DetailExaminable
{
    [RegisterComponent]
    public sealed partial class DetailExaminableComponent : Component
    {
        [DataField(required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public string Content = string.Empty;

        [DataField(required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public string NsfwContent = string.Empty;

        // DEN - Self-examination text
        [DataField]
        [ViewVariables(VVAccess.ReadWrite)]
        public string SelfContent = string.Empty;
    }
}
