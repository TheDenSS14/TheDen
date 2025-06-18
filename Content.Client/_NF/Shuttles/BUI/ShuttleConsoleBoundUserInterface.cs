// SPDX-FileCopyrightText: 2025 neuPanda <chriseparton@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

// NeuPanda - This file is licensed under AGPLv3
// Copyright (c) 2025 NeuPanda
// See AGPLv3.txt for details.
using Content.Client.Shuttles.UI;
using Content.Shared._NF.Shuttles.Events;

namespace Content.Client.Shuttles.BUI
{
    public sealed partial class ShuttleConsoleBoundUserInterface
    {
        private void NfOpen()
        {
            _window ??= new ShuttleConsoleWindow();
            _window.OnInertiaDampeningModeChanged += OnInertiaDampeningModeChanged;
        }
        private void OnInertiaDampeningModeChanged(NetEntity? entityUid, InertiaDampeningMode mode)
        {
            SendMessage(new SetInertiaDampeningRequest
            {
                ShuttleEntityUid = entityUid,
                Mode = mode,
            });
        }

    }
}
