// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Psionics;
using Content.Shared.Eui;
using Content.Server.EUI;
using Content.Server.Abilities.Psionics;

namespace Content.Server.Psionics
{
    public sealed class AcceptPsionicsEui : BaseEui
    {
        private readonly PsionicAbilitiesSystem _psionicsSystem;
        private readonly EntityUid _entity;

        public AcceptPsionicsEui(EntityUid entity, PsionicAbilitiesSystem psionicsSys)
        {
            _entity = entity;
            _psionicsSystem = psionicsSys;
        }

        public override void HandleMessage(EuiMessageBase msg)
        {
            base.HandleMessage(msg);

            if (msg is not AcceptPsionicsChoiceMessage choice ||
                choice.Button == AcceptPsionicsUiButton.Deny)
            {
                Close();
                return;
            }

            _psionicsSystem.AddRandomPsionicPower(_entity);
            Close();
        }
    }
}
