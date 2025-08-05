// SPDX-FileCopyrightText: 2025 little-meow-meow
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._starcup.AACTablet;

namespace Content.Client._DV.AACTablet.UI;

public sealed partial class AACBoundUserInterface
{
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not AACTabletBuiState msg)
            return;

        _window?.Update(msg);
    }
}
