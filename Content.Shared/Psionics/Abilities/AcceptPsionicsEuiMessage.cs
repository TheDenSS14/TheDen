// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Psionics
{
    [Serializable, NetSerializable]
    public enum AcceptPsionicsUiButton
    {
        Deny,
        Accept,
    }

    [Serializable, NetSerializable]
    public sealed class AcceptPsionicsChoiceMessage : EuiMessageBase
    {
        public readonly AcceptPsionicsUiButton Button;

        public AcceptPsionicsChoiceMessage(AcceptPsionicsUiButton button)
        {
            Button = button;
        }
    }
}
