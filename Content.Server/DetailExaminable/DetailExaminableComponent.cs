// SPDX-FileCopyrightText: 2022 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.DetailExaminable
{
    [RegisterComponent]
    public sealed partial class DetailExaminableComponent : Component
    {
        [DataField(required: true)] [ViewVariables(VVAccess.ReadWrite)]
        public string Content = string.Empty;

        [DataField(required: true)] [ViewVariables(VVAccess.ReadWrite)]
        public string NsfwContent = string.Empty;
    }
}
