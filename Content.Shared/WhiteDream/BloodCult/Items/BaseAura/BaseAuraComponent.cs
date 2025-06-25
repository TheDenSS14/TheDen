// SPDX-FileCopyrightText: 2025 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chat;

namespace Content.Shared.WhiteDream.BloodCult.Items.BaseAura;

public abstract partial class BaseAuraComponent : Component
{
    [DataField]
    public string? Speech;

    [DataField]
    public InGameICChatType ChatType = InGameICChatType.Whisper;
}
