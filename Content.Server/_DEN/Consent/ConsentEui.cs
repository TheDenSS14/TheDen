// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.EUI;

namespace Content.Server._DEN.Consent;

public sealed class ConsentEui : BaseEui
{
    // TODO make the lobby consent ui button use this as well ppbtbbbbfttt faaart fartt fartttt ill get to it
    public ConsentEui()
    {
        IoCManager.InjectDependencies(this);
    }
}
