// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Content.Shared.CCVar;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Robust.Shared.Utility;

namespace Content.Client.Players.PlayTimeTracking;

public sealed partial class JobRequirementsManager
{
    private bool _whitelisted = false;

    private void RxWhitelist(MsgWhitelist message)
    {
        _sawmill.Debug($"Received new whitelist status: {message.Whitelisted}, previously {_whitelisted}");
        _whitelisted = message.Whitelisted;
    }

    public bool IsWhitelisted()
    {
        return _whitelisted;
    }
}
