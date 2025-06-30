// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Eui;
using Content.Shared.Psionics;
using JetBrains.Annotations;
using Robust.Client.Graphics;

namespace Content.Client.Psionics.UI
{
    [UsedImplicitly]
    public sealed class AcceptPsionicsEui : BaseEui
    {
        private readonly AcceptPsionicsWindow _window;

        public AcceptPsionicsEui()
        {
            _window = new AcceptPsionicsWindow();

            _window.DenyButton.OnPressed += _ =>
            {
                SendMessage(new AcceptPsionicsChoiceMessage(AcceptPsionicsUiButton.Deny));
                _window.Close();
            };

            _window.AcceptButton.OnPressed += _ =>
            {
                SendMessage(new AcceptPsionicsChoiceMessage(AcceptPsionicsUiButton.Accept));
                _window.Close();
            };
        }

        public override void Opened()
        {
            IoCManager.Resolve<IClyde>().RequestWindowAttention();
            _window.OpenCentered();
        }

        public override void Closed()
        {
            _window.Close();
        }

    }
}
